using Senoc.Model.Eventing;
using Senoc.Model.Events;

namespace Senoc.Model.Primitives
{
    public interface IRoutable : 
        IIdentifiable, 
        IClockedElement,
        IListenTo<SimulationStartedEvent>.All,
        IListenTo<FlitEvent>.SentToMe,
        IListenTo<FlitAcceptedEvent>.SentToMe,
        IListenTo<FlitDeclinedEvent>.SentToMe
    {
        int X { get; set; }
        int Y { get; set; }
    }
}