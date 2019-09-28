using System.Collections.Generic;

namespace Waremap.Models
{
    public class State
    {
        public Geo Geo { get; set; } = new Geo();
        public Equipment Equipment { get; set; } = new Equipment();
    }
    
    public class Node
    {
        public int Id { get; set; }
        public NodeType Type { get; set; }
        public int Depot { get; set; }
        public int Floor { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public List<int> OperationIds { get; set; } = new List<int>();
    }

    public class Operation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProcessingTime { get; set; }
    }

    public class Edge
    {
        public int From { get; set; }
        public int To { get; set; }
        public EdgeType Type { get; set; }
        public int Weight { get; set; }
    }

    public class Depot
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public int Floor { get; set; }
        public string Name { get; set; }
    }

    public class Geo
    {
        public List<Node> Nodes { get; set; } = new List<Node>();
        public List<Edge> Edges { get; set; } = new List<Edge>();
        public List<Depot> Depots { get; set; } = new List<Depot>();
    }


    public class Waypoint
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public int OperationId { get; set; }
        public int StarTime { get; set; }
        public int EndTime { get; set; }
    }

    public class Part
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public int AssemblyId { get; set; }
        public List<Waypoint> Path { get; set; } = new List<Waypoint>();
    }

    public class Assembly
    {
        public int Id { get; set; }
        public int Name { get; set; }
    }

    public class Equipment
    {
        public List<Operation> Operations { get; set; } = new List<Operation>();
        public List<Part> Parts { get; set; } = new List<Part>();
        public List<Assembly> Assemblies { get; set; } = new List<Assembly>();
    }

    public enum NodeType
    {
        Machine,
        Point
    }
    public enum EdgeType
    {
        Road,
        Elevator,
        Ladder,
        Footway
    }
}