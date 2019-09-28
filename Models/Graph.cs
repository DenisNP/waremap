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
            Edges = new Edge[geo.Nodes.Count, geo.Nodes.Count];
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
    }
}