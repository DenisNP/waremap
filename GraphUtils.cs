using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Waremap.Models;

namespace Waremap
{
    public static class GraphUtils
    {
        public static List<int> FindNeighbours(Graph graph, int nodeId, List<int> exclude, bool onlyCore)
        {
            var edges = graph.EdgesAsList.Where(e => e.From == nodeId || e.To == nodeId).ToList();

            return edges
                .Select(e => e.From)
                .Concat(edges.Select(e => e.To))
                .Distinct()
                .Where(nId => !exclude.Contains(nId) && nId != nodeId && (!onlyCore || graph.Nodes[nId].NeedIsCore()))
                .ToList();
        }
        
        public static List<Node> FindCore(Graph graph, params EdgeType[] types)
        {
            var edges = graph.EdgesAsList.Where(e => types.Contains(e.Type)).ToList();
            if (edges.Count == 0)
            {
                return new List<Node>();
            }

            var nodeIds = edges
                .Select(e => e.From)
                .Concat(edges.Select(e => e.To))
                .Distinct()
                .ToList();

            var seen = new List<int>();
            for (var i = 0; i < nodeIds.Count; i++)
            {
                seen.Add(nodeIds[i]);
                var neighbours = FindNeighbours(graph, nodeIds[i], nodeIds.GetRange(0, i), false);
                seen.AddRange(neighbours.Where(nId => nodeIds.Contains(nId)));
            }

            if (seen.Distinct().Count() == nodeIds.Count)
            {
                return nodeIds.Select(nId => graph.Nodes[nId]).ToList();
            }
            
            return new List<Node>();
        }

        public static PathToNode FindClosestWithCriteria(
            Graph graph,
            int startNode,
            Predicate<int> criteria,
            List<int> seen,
            bool onlyCore
        )
        {
            if (criteria.Invoke(startNode))
            {
                return new PathToNode
                {
                    Path = new List<int>{startNode},
                    Weight = 0
                };
            }
            
            var neighbours = FindNeighbours(graph, startNode, seen, onlyCore);
            var closest = new List<PathToNode>();
            
            var newSeen = seen.Concat(neighbours).ToList();
            newSeen.Add(startNode);

            foreach (var neighbour in neighbours)
            {
                var closestCore = FindClosestWithCriteria(graph, neighbour, criteria, newSeen, onlyCore);
                if (closestCore.Target() != -1)
                {
                    var edge = graph.Edges[neighbour, startNode];
                    closest.Add(new PathToNode
                    {
                        Path = new List<int> {startNode}.Concat(closestCore.Path).ToList(),
                        Weight = edge.Weight + closestCore.Weight
                    });
                }
            }
            
            if (closest.Count > 0)
            {
                return closest.MinBy(path => path.Weight).First();
            }

            return new PathToNode{Path = new List<int>(), Weight = -1};
        }

        public static PathToNode FindClosestCore(Graph graph, int nodeId, List<int> coreIds)
        {
            return FindClosestWithCriteria(graph, nodeId, coreIds.Contains, new List<int>(), false);
        }

        public static void AssignClosestCores(Graph graph, List<int> coreIds)
        {
            foreach (var node in graph.Nodes.Values)
            {
                if (!coreIds.Contains(node.Id) && node.Type == NodeType.Machine)
                {
                    var cCore = FindClosestCore(graph, node.Id, coreIds);
                    if (cCore.Target() != -1)
                    {
                        node.AssignClosestCore(cCore);
                        graph.Nodes[cCore.Target()].AssignClosestFor(cCore.GetReverse());
                    }
                }
            }
        }

        public class PathToNode
        {
            public List<int> Path = new List<int>();
            public int Weight;

            public int Target()
            { 
                return Path.Count == 0 ? -1 : Path.Last();
            }

            public PathToNode GetReverse()
            {
                var ptn = new PathToNode();
                foreach (var p in Path)
                {
                    ptn.Path.Insert(0, p);
                }

                ptn.Weight = Weight;
                return ptn;
            }

            public void AddToWaypoints(List<Waypoint> waypoint, bool reversed = false)
            {
                var list = new List<int>(Path);
                if (list.Count == 0) return;
                
                if (reversed) list.Reverse();
                for (var i = 0; i < list.Count - 1; i++)
                {
                    waypoint.Add(new Waypoint
                    {
                        FromNode = list[i],
                        ToNode = list[i + 1]
                    });
                }
                
                waypoint.Add(new Waypoint
                {
                    FromNode = list.Last(),
                    ToNode = list.Last()
                });
            }
        }
    }
}