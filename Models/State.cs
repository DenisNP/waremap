using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Waremap.Models
{
    public class State
    {
        public Geo Geo { get; set; } = new Geo();
        public Equipment Equipment { get; set; } = new Equipment();
        public Roadmap CarRoadmap { get; set; } = new Roadmap();

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

        private GraphUtils.PathToNode _closestCore;
        private GraphUtils.PathToNode _closestFor;
        private bool _isCore;

        public void AssignClosestCore(GraphUtils.PathToNode v)
        {
            _closestCore = v;
        }

        public GraphUtils.PathToNode NeedClosestCore()
        {
            return _closestCore;
        }

        public void AssignClosestFor(GraphUtils.PathToNode v)
        {
            _closestFor = v;
        }

        public GraphUtils.PathToNode NeedClosestFor()
        {
            return _closestFor;
        }

        public void AssignIsCore(bool v)
        {
            _isCore = v;
        }

        public bool NeedIsCore()
        {
            return _isCore;
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
        public int Order { get; set; }
        public int OperationId { get; set; }
    }

    public class Roadmap
    {
        public List<Waypoint> Path { get; set; } = new List<Waypoint>();
        public int Position { get; set; } = 0;

        public Waypoint CurrentWaypoint()
        {
            return Path.Count == 0 ? null : Path[Position];
        }

        public Waypoint NextWaypoint()
        {
            if (Path.Count == 0) return null;
            return Position < Path.Count - 1 ? Path[Position + 1] : Path[Position];
        }

        public Waypoint GoToNext()
        {
            var next = NextWaypoint();
            if (next == null) return null;
            Position = Path.IndexOf(next);
            return CurrentWaypoint();
        }

        public void SetWaypoint(Waypoint wp)
        {
            if (Path.Contains(wp))
            {
                Position = Path.IndexOf(wp);
            }
            else
            {
                Path.Insert(Position + 1, wp);
                Position++;
            }
        }
    }

    public class Part
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public int AssemblyId { get; set; }
        public Roadmap Roadmap { get; set; } = new Roadmap();
        public List<Process> Process { get; set; } = new List<Process>();
    }

    public class Waypoint
    {
        public int FromNode { get; set; }
        public int ToNode { get; set; }
        public int OperationId { get; set; }
        public bool OffWay { get; set; }
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
        public string GetOperationById(int id)
        {
            return Operations.First(x => x.Id == id).Name;
        }
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