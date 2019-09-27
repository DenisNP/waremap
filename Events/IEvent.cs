namespace Waremap.Events
{
    public interface IEvent
    {
        EventType Type { get; set; }
    }

    public enum EventType
    {
        AddNode,
        RemoveNode,
        AddEdge,
        RemoveEdge,
    }
}