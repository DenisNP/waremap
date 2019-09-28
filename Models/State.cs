using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Waremap.Models
{
    public class State
    {
        public Geo Geo { get; set; } = new Geo();
        public Equipment Equipment { get; set; } = new Equipment();
        public List<Waypoint> CarWaypoints { get; set; } = new List<Waypoint>();
        public int CarPosition { get; set; } = 0;

        [JsonIgnore]
        public Dictionary<int, string> Background { get; set; } = new Dictionary<int, string>();
    }
    
    public class Node
    {
        public int Id { get; set; }
        public NodeType Type { get; set; }
        public int Depot { get; set; }
        public int Floor { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }
        public List<int> OperationIds { get; set; } = new List<int>();
        public string Icon { get; set; }

        private int _closestCore;

        public void AssignClosestCore(int v)
        {
            _closestCore = v;
            Console.WriteLine($"For {Id} closest is {v}");
        }

        public int NeedClosestCore()
        {
            return _closestCore;
        }
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
    
    public class Process
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public int OperationId { get; set; }
    }

    public class Part
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public int AssemblyId { get; set; }
        public Waypoint Waypoint { get; set; }
        public List<Process> Path { get; set; } = new List<Process>();
    }

    public class Waypoint
    {
        public int FromNode { get; set; }
        public int ToNode { get; set; }
        public int TimeLeft { get; set; }
        public int TimeTotal { get; set; }
        public int OperationId { get; set; }
    }

    public class Assembly
    {
        public int Id { get; set; }
        public string Name { get; set; }
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