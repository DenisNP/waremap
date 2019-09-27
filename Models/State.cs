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
    }

    public class Edge
    {
        public int From { get; set; }
        public int To { get; set; }
        public EdgeType Type { get; set; }
        public int Weight { get; set; }
    }

    public class Geo
    {
        public List<Node> Nodes { get; set; } = new List<Node>();
        public List<Edge> Edges { get; set; } = new List<Edge>();
    }

    public class Car
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int TotalCapacity { get; set; }
        public int FreeCapacity { get; set; }
        public int FromNodeId { get; set; }
        public int ToNodeId { get; set; }
        public double Progress { get; set; }
    }

    public class Waypoint
    {
        public int NodeId { get; set; }
        public int ProcessingTime { get; set; }
    }

    public class Part
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public List<Waypoint> Path { get; set; } = new List<Waypoint>();
    }

    public class Equipment
    {
        public List<Car> Cars { get; set; } = new List<Car>();
        public List<Part> Parts { get; set; } = new List<Part>();
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
        Ladder
    }
}