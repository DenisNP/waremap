using System.Collections.Generic;
using System.Linq;
using Waremap.Models;

namespace Waremap.Events
{
    #nullable enable
    public class EventAddNode : IEvent
    {
        public int? Id { get; set; }
        public NodeType Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Floor { get; set; }
        public int Depot { get; set; }
        
        public string? Name { get; set; }
        public string? Icon { get; set; }
        public List<int>? OperationIds { get; set; }

        public string GetEventName(State state)
        {
            var newName = "";

            switch (Icon)
            {
                case "Node":
                    newName = "Точка"; break;
                case "Machine":
                    newName = $"Участок {state.Equipment.GetOperationById(OperationIds.First())}"; break;
                case "Elevator": break;
                    newName = "Лифт в"; break;
                case "Ladder": break;
                    newName = "Лестница в"; break;
                case "Door": break;
                    newName = "Дверь в"; break;
            }

            return Name ?? $"{newName} {Id}, Цех {Depot}, Этаж {Floor}";
        }
        public void Run(State state)
        {
            var node = state.Geo.Nodes.FirstOrDefault(n => n.Id == Id);
            if (node != null)
            {
                // update node
                node.Type = Type;
                node.X = X;
                node.Y = Y;
                node.Floor = Floor;
                node.Depot = Depot;
                node.Icon = Icon ?? node.Icon;
                node.Name = Name ?? node.Name;
                if (OperationIds != null)
                    node.OperationIds = OperationIds;
            }
            else
            {
                // add node
                state.Geo.Nodes.Add(new Node
                {
                    Id = Utils.CreateIdFor(state.Geo.Nodes.Select(n => n.Id).ToList()),
                    Depot = Depot,
                    Floor = Floor,
                    Type = Type,
                    X = X,
                    Y = Y,
                    Icon = Icon ?? "",
                    Name = GetEventName(state),
                    OperationIds = OperationIds ?? new List<int>() 
                });
            }

            EventAddDepot.RedefineDepots(state);
        }
    }
    #nullable disable
}