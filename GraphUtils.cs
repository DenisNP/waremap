using System.Collections.Generic;
using System.Linq;
using Waremap.Models;

namespace Waremap
{
    public static class GraphUtils
    {
        public static List<int> FindNeighbours(Graph graph, int nodeId, List<int> exclude)
        {
            var edges = graph.Edges.Where(e => e.From == nodeId || e.To == nodeId).ToList();
            var ids = exclude.Concat(new List<int>(nodeId));

            return edges
                .Select(e => e.From)
                .Concat(edges.Select(e => e.To))
                .Distinct()
                .Where(nId => !ids.Contains(nId))
                .ToList();
        }
        
        public static List<Node> FindCore(Graph graph, params EdgeType[] types)
        {
            var edges = graph.Edges.Where(e => types.Contains(e.Type)).ToList();
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

        public static int FindClosestCore(Graph graph, int nodeId, List<int> coreIds, List<int> seen)
        {
            if (coreIds.Contains(nodeId)) return nodeId;
            var neighbours = FindNeighbours(graph, nodeId, seen);
            
            foreach (var neighbour in neighbours)
            {
                if (coreIds.Contains(neighbour))
                {
                    return neighbour;
                }
            }

            var newSeen = seen.Concat(neighbours).ToList();
            newSeen.Add(nodeId);

            foreach (var neighbour in neighbours)
            {
                var closestCore = FindClosestCore(graph, neighbour, coreIds, newSeen);
                if (closestCore != -1)
                {
                    return closestCore;
                }
            }

            return -1;
        }

        public static void AssignClosestCores(Graph graph, List<int> coreIds)
        {
            foreach (var node in graph.Nodes.Values)
            {
                if (!coreIds.Contains(node.Id) && node.Type == NodeType.Machine)
                {
                    node.AssignClosestCore(FindClosestCore(graph, node.Id, coreIds, new List<int>()));
                }
            }
        }
    }
}