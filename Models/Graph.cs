using System.Collections.Generic;

namespace Waremap.Models
{
    public class Graph
    {
        public List<Edge> Edges;
        public Dictionary<int, Node> Nodes;

        public Graph(Geo geo)
        {
            Edges = geo.Edges;
            Nodes = new Dictionary<int, Node>();
            geo.Nodes.ForEach(n =>
            {
                Nodes.Add(n.Id, n);
            });
        }
    }
}