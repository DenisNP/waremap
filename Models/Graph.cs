using System;
using System.Collections.Generic;
using System.Linq;

namespace Waremap.Models
{
    public class Graph
    {
        public Edge[,] Edges;
        public Dictionary<int, Node> Nodes;
        public List<Edge> EdgesAsList;

        public Graph(Geo geo)
        {
            var maxNodeId = geo.Nodes.Select(n => n.Id).Max();
            Edges = new Edge[maxNodeId + 1, maxNodeId + 1];
            geo.Edges.ForEach(e =>
            {
                Edges[e.From, e.To] = e;
                Edges[e.To, e.From] = e;
            });
            EdgesAsList = geo.Edges;
            
            Nodes = new Dictionary<int, Node>();
            geo.Nodes.ForEach(n =>
            {
                Nodes.Add(n.Id, n);
            });
        }

        public double DistFromWeight(int from, int to)
        {
            var edge = Edges[from, to];
            return 10.0 / edge.Weight;
        }
    }
}