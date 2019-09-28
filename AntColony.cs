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
        private const double TakeInFood = 5.0;
        private const double TakeOutFood = 15.0;
        private readonly double[,] _food;

        public AntColony(Graph graph)
        {
            _food = new double[graph.Nodes.Count, graph.Nodes.Count];
            for (var i = 0; i < graph.Nodes.Count; i++)
            {
                for (var j = 0; j < graph.Nodes.Count; j++)
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
                var carNode = route.CarWaypoints.Last().FromNode;
                var potential = GraphUtils.FindNeighbours(route.Graph, carNode, new List<int>());
                var probs = new List<double>();
                foreach (var neighbour in potential)
                {
                    var edgeFood = _food[carNode, neighbour];
                    probs.Add(edgeFood * route.Graph.DistFromWeight(neighbour, carNode));
                }

                var sumProb = probs.Sum();
                var index = GraphUtils.SelectFromRoulette(probs.Select(p => p / sumProb).ToArray(), random);
                if (index == -1)
                {
                    index = random.Next(0, probs.Count);
                }

                // take food from node
                var nextNodeId = potential[index];
                var time = AddTime(route.Graph.Edges[carNode, nextNodeId]);
                route.Time += time;
                var nextNode = route.Graph.Nodes[nextNodeId];
                currentFood += TakeInPart(route, nextNode.NeedClosestCore().NId, time);
                var (addFood, waypoint) = TakeOutPart(route, nextNode.NeedClosestCore().NId);
                currentFood += addFood;
                
                // add edge
                route.CarWaypoints.Add(new Waypoint
                {
                    FromNode = carNode,
                    ToNode = nextNodeId,
                    OperationId = 0
                });
                currentEdges.Add((carNode, nextNodeId));
                currentWeight += route.Graph.Edges[carNode, nextNodeId].Weight;
                
                // machine waypoint
                if (waypoint != null)
                    route.CarWaypoints.Add(waypoint);
                
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

        private (double, Waypoint) TakeOutPart(Route route, int node)
        {
            var partsOnCar = new List<int>();
            foreach (var pPos in route.PartPositions)
            {
                if (pPos.Value.ToNode == node)
                {
                    return (0.0, null);
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
                        route.Graph.Nodes[node].OperationIds.Contains(op.OperationId));
                    if (firstOpMatch != null)
                    {
                        route.OperationsLeft[i].Remove(firstOpMatch);
                        var waypoint = new Waypoint
                        {
                            FromNode = node,
                            ToNode = node,
                            OperationId = firstOpMatch.OperationId
                        };
                        route.Time += route.Operations[firstOpMatch.OperationId].ProcessingTime;
                        return (TakeOutFood, waypoint);
                    }
                }
            }
            
            return (0.0, null);
        }

        private double TakeInPart(Route route, int node, int time)
        {
            foreach (var pPos in route.PartPositions)
            {
                if (pPos.Value.ToNode == node && pPos.Value.EndTime <= route.Time)
                {
                    // take part back
                    route.PartPositions[pPos.Key].ToNode = 0;
                    route.PartPositions[pPos.Key].EndTime = route.Time + time;
                    return TakeInFood;
                }
            }

            return 0.0;
        }

        private int AddTime(Edge edge)
        {
            return (int)Math.Round(edge.Weight / ManSpeed);
        }
    }
}