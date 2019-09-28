using System;
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
                // var neighbours = FindNeighbours(graph, nodeIds[i], nodeIds[0..i]);
            }
            
            throw new NotImplementedException();
        }
    }
}