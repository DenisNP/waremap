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
                
                var possibleNodes = graph.NodesAsList().OrderBy(node =>
                {
                    if (node.Type != NodeType.Machine) return 99999;
                    GraphUtils.PathToNode pathTo = null;
                    if (node.NeedIsCore())
                    {
                        pathTo = GraphUtils.FindClosestWithCriteria(
                            graph,
                            carNode,
                            i => i == node.Id,
                            new List<int>(),
                            true
                        );
                    }
                    else
                    {
                        if (node.NeedClosestCore() != null && node.NeedClosestCore().Target() != -1)
                        {
                            node = graph.Nodes[node.NeedClosestCore().Target()];
                            pathTo = GraphUtils.FindClosestWithCriteria(
                                graph,
                                carNode,
                                i => i == node.Id,
                                new List<int>(),
                                true
                            );
                        }
                    }

                    if (pathTo == null || pathTo.Target() == -1)
                    {
                        return 99998;
                    }

                    var coeff = GetBusyTime(busyNodes, node.Id, t) - WeightToTime(pathTo.Weight);
                    if (node.OperationIds.Contains(nextOperation.OperationId))
                    {
                        return coeff;
                    }

                    return coeff * 2;
                });

                var nextNode = possibleNodes.First();
                var pathTo = GraphUtils.FindClosestWithCriteria(
                    graph,
                    carNode,
                    i => i == nextNode.Id,
                    new List<int>(),
                    true
                );
                if (pathTo == null || pathTo.Target() == -1)
                {
                    pathTo = GraphUtils.FindClosestWithCriteria(
                        graph,
                        carNode,
                        i => i == nextNode.Id,
                        new List<int>(),
                        false
                    );
                }

                carNode = nextNode.Id;
                foreach (var prt in partsOnCar)
                {
                    pathTo.AddToWaypoints(waypoints[prt]);
                }
                t += WeightToTime(pathTo.Weight);
                var diff = GetBusyTime(busyNodes, carNode, t);
                if (diff > 0)
                {
                    t += diff;
                }

                if (busyNodes.ContainsKey(carNode))
                {
                    partsOnCar.Add(busyNodes[carNode].PartId);
                    busyNodes.Remove(carNode);
                }
            }

            return (100.0 / t, waypoints);
        }

        private static int GetBusyTime(Dictionary<int, BusyNode> busyNodes, int node, int time)
        {
            if (!busyNodes.ContainsKey(node))
            {
                return 0;
            }

            return busyNodes[node].EndTime - time;
        }

        private static bool NeedPickup(Dictionary<int, BusyNode> busyNodes, int node, int time)
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