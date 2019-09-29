using System;
using System.Collections.Generic;
using System.Linq;
using Waremap.Models;

namespace Waremap
{
    public class AntColony
    {
        private const double InitialFood = 1.0;
        private const double ManSpeed = 12.0;
        private const double TakeInFood = 500.0;
        private const double TakeOutFood = 1500.0;
        private readonly double[,] _food;

        public AntColony(Graph graph)
        {
            var maxNodeId = graph.Nodes.Values.Select(n => n.Id).Max();
            _food = new double[maxNodeId + 1, maxNodeId + 1];
            for (var i = 0; i < maxNodeId + 1; i++)
            {
                for (var j = 0; j < maxNodeId + 1; j++)
                {
                    _food[i, j] = InitialFood;
                }
            }
        }
        
        public void RunAnt(Route route)
        {
            var random = new Random();
            var steps = 1000;
            
            var currentFood = 0.0;
            var currentEdges = new List<(int, int)>();
            var currentWeight = 0.0;
                
            while (steps-- > 0)
            {
                var carNode = route.CarWaypoints.Last().ToNode;
                var potential = GraphUtils.FindNeighbours(route.Graph, carNode, new List<int>()).Where(nId => nId != carNode).ToList();
                var probs = new List<double>();
                foreach (var neighbour in potential)
                {
                    var edgeFood = _food[carNode, neighbour];
                    var edgeW = route.Graph.DistFromWeight(neighbour, carNode);
                    probs.Add(edgeFood * edgeW);
                }

                var sumProb = probs.Sum();
                var index = GraphUtils.SelectFromRoulette(probs.Select(p => p / sumProb).ToArray(), random);
                if (index == -1)
                {
                    index = random.Next(0, probs.Count);
                }

                // take food from node
                var nextNodeId = potential[index];
                
                // add edge
                route.CarWaypoints.Add(new Waypoint
                {
                    FromNode = carNode,
                    ToNode = nextNodeId,
                    OperationId = 0
                });
                currentEdges.Add((carNode, nextNodeId));
                currentWeight += route.Graph.Edges[carNode, nextNodeId].Weight;

                // add machine process if needed
                var time = WeightToTime(route.Graph.Edges[carNode, nextNodeId].Weight);
                route.Time += time;
                var nextNode = route.Graph.Nodes.Values.FirstOrDefault(n => n.NeedClosestCore().NId == nextNodeId);
                if (nextNode == null)
                {
                    nextNode = route.Graph.Nodes[nextNodeId];
                }
                currentFood += TakeInPart(route, nextNode.NeedClosestCore(), nextNode.Id, time);
                currentFood += TakeOutPart(route, nextNode.NeedClosestCore(), nextNode.Id);

                // if exit
                if (!route.OperationsLeft.Values.SelectMany(x => x).Any())
                {
                    // no operations left
                    break;
                }
            }

            var addAllFood = currentFood / currentWeight;
            foreach (var currentEdge in currentEdges)
            {
                _food[currentEdge.Item1, currentEdge.Item2] += addAllFood;
                _food[currentEdge.Item2, currentEdge.Item1] += addAllFood;
            }

            route.Result = 1.0 / route.Time + 5.0 / addAllFood;
        }

        private double TakeOutPart(Route route, GraphUtils.PathToNode path, int targetNode)
        {
            var partsOnCar = new List<int>();
            foreach (var pPos in route.PartPositions)
            {
                if (pPos.Value.ToNode == targetNode)
                {
                    return 0.0;
                }

                if (pPos.Value.ToNode == 0)
                {
                    partsOnCar.Add(pPos.Key);
                }
            }
            
            foreach (var i in partsOnCar)
            {
                var opsLeft = route.OperationsLeft[i];
                if (opsLeft.Count > 0)
                {
                    var first = opsLeft.First();
                    var opsNeeded = opsLeft.Where(op => op.Order == first.Order);
                    var firstOpMatch = opsNeeded.FirstOrDefault(op =>
                        route.Graph.Nodes[targetNode].OperationIds.Contains(op.OperationId));
                    if (firstOpMatch != null)
                    {
                        route.OperationsLeft[i].Remove(firstOpMatch);
                        // TODO add forward waypoints
                        
                        // add machine waypoint
                        var waypoint = new Waypoint
                        {
                            FromNode = targetNode,
                            ToNode = targetNode,
                            OperationId = firstOpMatch.OperationId
                        };
                        
                        var time = route.Operations[firstOpMatch.OperationId].ProcessingTime + WeightToTime(path.Weight);
                        route.PartPositions[i].ToNode = targetNode;
                        route.PartPositions[i].EndTime = route.Time + time;
                        return TakeOutFood;
                    }
                }
            }

            return 0.0;
        }

        private double TakeInPart(Route route, GraphUtils.PathToNode node, int nextNodeId, int time)
        {
            foreach (var pPos in route.PartPositions)
            {
                if (pPos.Value.ToNode == nextNodeId && pPos.Value.EndTime <= route.Time)
                {
                    // take part back
                    route.PartPositions[pPos.Key].ToNode = 0;
                    route.Time += WeightToTime(node.Weight);
                    // TODO add back waypoints here
                    
                    //
                    route.PartPositions[pPos.Key].EndTime = route.Time + time;
                    return TakeInFood;
                }
            }

            return 0.0;
        }

        private int WeightToTime(int w)
        {
            return (int)Math.Round(w / ManSpeed);
        }
    }
}