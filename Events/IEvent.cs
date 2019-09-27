namespace Waremap.Events
{
    public interface IEvent
    {
        EventType Type { get; set; }
    }

    public enum EventType
    {
        AddDepot,
        RemoveDepot,
        AddNode,
        RemoveNode,
        AddEdge,
        RemoveEdge,
    }
}