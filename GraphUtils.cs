using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Waremap.Models;

namespace Waremap
{
    public static class GraphUtils
    {
        public static List<int> FindNeighbours(Graph graph, int nodeId, List<int> exclude)
        {
            var edges = graph.EdgesAsList.Where(e => e.From == nodeId || e.To == nodeId).ToList();

            return edges
                .Select(e => e.From)
                .Concat(edges.Select(e => e.To))
                .Distinct()
                .Where(nId => !exclude.Contains(nId) && nId != nodeId)
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
                var neighbours = FindNeighbours(graph, nodeIds[i], nodeIds.GetRange(0, i));
                seen.AddRange(neighbours.Where(nId => nodeIds.Contains(nId)));
            }

            if (seen.Distinct().Count() == nodeIds.Count)
            {
                return nodeIds.Select(nId => graph.Nodes[nId]).ToList();
            }
            
            return new List<Node>();
        }

        public static PathToNode FindClosestCore(Graph graph, int nodeId, List<int> coreIds, List<int> seen)
        {
            if (coreIds.Contains(nodeId)) return new PathToNode {NId = nodeId, Weight = 0};
            var neighbours = FindNeighbours(graph, nodeId, seen);

            var closest = new List<PathToNode>();
            foreach (var neighbour in neighbours)
            {
                if (coreIds.Contains(neighbour))
                {
                    var edge = graph.Edges[neighbour, nodeId];
                    closest.Add(new PathToNode{NId = neighbour, Weight = edge.Weight});
                }
            }

            if (closest.Count > 0)
            {
                return closest.MinBy(path => path.Weight).First();
            }

            var newSeen = seen.Concat(neighbours).ToList();
            newSeen.Add(nodeId);

            foreach (var neighbour in neighbours)
            {
                var closestCore = FindClosestCore(graph, neighbour, coreIds, newSeen);
                if (closestCore.NId != -1)
                {
                    var edge = graph.Edges[neighbour, nodeId];
                    closest.Add(new PathToNode{NId = neighbour, Weight = edge.Weight + closestCore.Weight});
                }
            }
            
            if (closest.Count > 0)
            {
                return closest.MinBy(path => path.Weight).First();
            }

            return new PathToNode{NId = -1, Weight = -1};
        }

        public static void AssignClosestCores(Graph graph, List<int> coreIds)
        {
            foreach (var node in graph.Nodes.Values)
            {
                if (!coreIds.Contains(node.Id) && node.Type == NodeType.Machine)
                {
                    node.AssignClosestCore(FindClosestCore(graph, node.Id, coreIds, new List<int>()));
                }
                else
                {
                    node.AssignClosestCore(new PathToNode{NId = node.Id, Weight = 0});
                }
            }
        }
        
        public static int SelectFromRoulette(double[] weight, Random rng) {
            double total = 0;
            var amount = rng.NextDouble();
            for(var a = 0; a < weight.Length; a++){
                total += weight[a];
                if(amount <= total){
                    return a;
                }
            }
            return -1;
        }

        public struct PathToNode
        {
            public int NId;
            public int Weight;
        }
    }
}