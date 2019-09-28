using System;
using System.Collections.Generic;
using System.Linq;
using Waremap.Models;

namespace Waremap
{
    public static class GraphUtils
    {
        public static List<int> FindNeighbours(Graph graph, Node node, List<Node> exclude)
        {
            var edges = graph.Edges.Where(e => e.From == node.Id || e.To == node.Id).ToList();
            var ids = exclude.Select(n => n.Id).ToList();
            ids.Add(node.Id);

            return edges
                .Select(e => e.From)
                .Concat(edges.Select(e => e.To))
                .Distinct()
                .Where(nId => !ids.Contains(nId))
                .ToList();
        }
        
        public static List<Node> FindCore(Graph graph, params EdgeType[] types)
        {
            var start = graph.Edges.FirstOrDefault(e => types.Contains(e.Type));
            if (start == null)
            {
                return new List<Node>();
            }

            var nodesLeft = graph.Nodes.Values.Where(n => n.Id != start.From && n.Id != start.To).ToList();
            var found = new List<Node>
            {
                graph.Nodes[start.From],
                graph.Nodes[start.To]
            };
            var nextNode = graph.Nodes[start.]

            while (nodesLeft.Count > 0)
            {
                
            }
            
            throw new NotImplementedException();
        }
    }
}