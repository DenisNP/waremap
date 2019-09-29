using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Waremap.Models;

namespace Waremap
{
    public class SimulatedAnnealing
    {
        private List<FullOperation> _lattice = new List<FullOperation>();
        private readonly Graph _graph;
        private readonly int _carStartNode;
        private readonly Random _random = new Random();

        public SimulatedAnnealing(Graph graph, State state)
        {
            _graph = graph;
            foreach (var part in state.Equipment.Parts)
            {
                var processes = part.Process.OrderBy(process => process.Order);
                var lastInsertIdx = -1;
                var lastInsertOrder = 0;
                foreach (var process in processes)
                {
                    int insertIndex;
                    if (process.Order == lastInsertOrder)
                    {
                        insertIndex = _random.Next(0, _lattice.Count + 1);
                        lastInsertIdx = Math.Max(lastInsertIdx + 1, insertIndex);
                    }
                    else
                    {
                        insertIndex = _random.Next(lastInsertIdx + 1, _lattice.Count + 1);
                        lastInsertIdx = insertIndex;
                    }

                    // Console.WriteLine(insertIndex + " " + _lattice.Count + " " + process.Order + " " + lastInsertOrder);
                    _lattice.Insert(lastInsertIdx, new FullOperation
                    {
                        OperationId = process.OperationId,
                        PartId = part.Id,
                        Order = process.Order,
                        OperationTime = state.Equipment.Operations.First(op => op.Id == process.OperationId).ProcessingTime
                    });
                }
            }

            _carStartNode = state.CarRoadmap.CurrentWaypoint().FromNode;
            Console.WriteLine("Operations created: " + JsonConvert.SerializeObject(_lattice));
        }

        public (double, Dictionary<int, List<Waypoint>>) Run()
        {
            return CalculateRoute(_graph, _lattice, _carStartNode);
        }
    
        public List<FullOperation> ShakeLattice() {
            var newLattice = new List<FullOperation>(_lattice);
            if (_lattice.Count <= 1) return newLattice;
            
            var firstIndex = _random.Next(0, _lattice.Count);
            var secondIndex = _random.Next(0, _lattice.Count);
            while (secondIndex == firstIndex)
            {
                secondIndex = _random.Next(0, _lattice.Count);
            }

            var firstOperation = newLattice[firstIndex];
            var secondOperation = newLattice[secondIndex];
            
            // repeat if cant switch
            while (firstOperation.PartId == secondOperation.PartId && firstOperation.Order != secondOperation.Order)
            {
                firstIndex = _random.Next(0, _lattice.Count);
                secondIndex = _random.Next(0, _lattice.Count);
                while (secondIndex == firstIndex)
                {
                    secondIndex = _random.Next(0, _lattice.Count);
                }
                firstOperation = newLattice[firstIndex];
                secondOperation = newLattice[secondIndex];
            }
            
            // switch operations
            newLattice[firstIndex] = secondOperation;
            newLattice[secondIndex] = firstOperation;

            return newLattice;
        }

        public static (double, Dictionary<int, List<Waypoint>>) CalculateRoute(Graph graph, List<FullOperation> lattice, int carStartNode)
        {
            var l = new List<FullOperation>(lattice);
            var t = 0;
            var busyNodes = new Dictionary<int, BusyNode>();
            var partsOnCar = lattice.Select(op => op.PartId).Distinct().ToHashSet();
            partsOnCar.Add(0);
            var carNode = carStartNode;
            var waypoints = new Dictionary<int, List<Waypoint>>();
            foreach (var prt in partsOnCar)
            {
                waypoints.Add(prt, new List<Waypoint>());
            }
            
            while (l.Count > 0)
            {
                var nextOperation = l[0];
                if (busyNodes.Count(bNode => bNode.Value.PartId == nextOperation.PartId) > 0)
                {
                    var busy = busyNodes.First(bn => bn.Value.PartId == nextOperation.PartId);
                    var bNode = graph.Nodes[busy.Key];
                    
                    // part busy, wait
                    var path = GraphUtils.FindClosestWithCriteria(graph, carNode, i =>
                    {
                        if (i == bNode.Id && bNode.Type == NodeType.Machine)
                        {
                            return true;
                        }
                        if (bNode.Type != NodeType.Machine)
                        {
                            var core = bNode.NeedClosestCore() != null ? bNode.NeedClosestCore().Target() : -1;
                            return core == i;
                        }
                        return false;
                    }, new List<int>());

                    t += path.Weight;
                    if (busy.Value.EndTime > t)
                    {
                        t = busy.Value.EndTime;
                    }

                    busyNodes.Remove(busy.Key);
                    foreach (var prt in partsOnCar)
                    {
                        path.AddToWaypoints(waypoints[prt]);
                    }
                    partsOnCar.Add(nextOperation.PartId);
                    continue;
                }

                var closest = GraphUtils.FindClosestWithCriteria(graph, carNode, nodeId =>
                {
                    var node = graph.Nodes[nodeId];
                    if (!node.NeedIsCore()) return false;
                    
                    if (node.Type != NodeType.Machine)
                    {
                        nodeId = node.NeedClosestFor() != null && node.NeedClosestFor().Path.Count != 0 ? node.NeedClosestFor().Target() : -1;
                        if (nodeId != -1)
                        {
                            node = graph.Nodes[nodeId];
                            if (node.OperationIds.Contains(nextOperation.OperationId))
                            {
                                var pathToThis = GraphUtils.FindClosestWithCriteria(
                                    graph,
                                    carNode,
                                    i => i == nodeId,
                                    new List<int>()
                                );
                                return !IsBusy(busyNodes, nodeId, t + WeightToTime(pathToThis.Weight));
                            }
                        }
                    }
                    else if (node.OperationIds.Contains(nextOperation.OperationId))
                    {
                        var pathToThis = GraphUtils.FindClosestWithCriteria(
                            graph,
                            carNode,
                            i => i == nodeId,
                            new List<int>()
                        );
                        return !IsBusy(busyNodes, nodeId, t + WeightToTime(pathToThis.Weight));
                    }
                    return false;
                }, new List<int>());
                
                // go car
                
                var nextNodeId = closest.Target();
                if (nextNodeId == -1)
                {
                    t += 300; // wait for 5 minutes
                    continue;
                }
                
                foreach (var prt in partsOnCar)
                {
                    closest.AddToWaypoints(waypoints[prt]);
                }
                t += WeightToTime(closest.Weight);
                
                //put part or take part
                var nextNode = graph.Nodes[nextNodeId];
                if (nextNode.Type != NodeType.Machine)
                {
                    nextNodeId = nextNode.NeedClosestFor().Target();
                    nextNode = graph.Nodes[nextNodeId];
                }

                var pathToTarget = GraphUtils.FindClosestWithCriteria(
                    graph,
                    closest.Target(),
                    i => i == nextNodeId,
                    new List<int>()
                );
                
                if (busyNodes.ContainsKey(nextNodeId))
                {
                    var takePart = busyNodes[nextNodeId];
                    busyNodes.Remove(nextNodeId);
                    partsOnCar.Add(takePart.PartId);
                    t += WeightToTime(pathToTarget.Weight);
                    pathToTarget.AddToWaypoints(waypoints[takePart.PartId], true);
                }

                if (nextNode.OperationIds.Contains(nextOperation.OperationId))
                {
                    t += WeightToTime(pathToTarget.Weight);
                    busyNodes.Add(nextOperation.PartId, new BusyNode
                    {
                        OperationId = nextOperation.OperationId,
                        PartId = nextOperation.PartId,
                        EndTime = t + nextOperation.OperationTime
                    });
                    pathToTarget.AddToWaypoints(waypoints[nextOperation.PartId]);
                    
                    l.RemoveAt(0);
                }

                carNode = closest.Target();
            }

            return (100.0 / t, waypoints);
        }

        private static bool IsBusy(Dictionary<int, BusyNode> busyNodes, int node, int time)
        {
            if (!busyNodes.ContainsKey(node))
            {
                return false;
            }

            return busyNodes[node].EndTime <= time;
        }

        private static int WeightToTime(int w)
        {
            return w / 12;
        }
    }

    public class FullOperation
    {
        public int OperationId { get; set; }
        public int PartId { get; set; }
        public int Order { get; set; }
        public int OperationTime { get; set; }
    }

    public class BusyNode
    {
        public int OperationId { get; set; }
        public int PartId { get; set; }
        public int EndTime { get; set; }
    }
}