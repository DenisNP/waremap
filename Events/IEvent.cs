using Waremap.Models;

namespace Waremap.Events
{
    public interface IEvent
    {
        void Run(State state);
    }
}