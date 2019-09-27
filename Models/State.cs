using System;
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

    public class Depot
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public int Floor { get; set; }
    }

    public class Geo
    {
        public List<Node> Nodes { get; set; } = new List<Node>();
        public List<Edge> Edges { get; set; } = new List<Edge>();
        public List<Depot> Depots { get; set; } = new List<Depot>();
    }

    public class Car
    {
        public int Id { get; set; }
        public CarType Type { get; set; }
        public int TotalCapacity { get; set; }
        public int FreeCapacity { get; set; }
        public int FromNodeId { get; set; }
        public int ToNodeId { get; set; }
        public double Progress { get; set; }
    }

    public class Waypoint
    {
        public int Id { get; set; }
        public int NodeId { get; set; }
        public int ProcessingTime { get; set; }
        public int StarTime { get; set; }
        public int EndTime { get; set; }
    }

    public class Part
    {
        public int Id { get; set; }
        public int CarId { get; set; }
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

    public enum CarType
    {
        Man,
        ManWithCar,
        Forklift
    }
}