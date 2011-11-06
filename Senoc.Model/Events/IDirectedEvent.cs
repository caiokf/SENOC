using Senoc.Model.Eventing;

namespace Senoc.Model.Events
{
    public interface IDirectedEvent<T> : IEvent
    {
        T CurrentSender { get; }
        T CurrentReceiver { get; }
    }
}