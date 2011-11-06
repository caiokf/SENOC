using Senoc.Model.Eventing;
using Senoc.Model.Primitives;

namespace Senoc.Model.Events
{
    public abstract class BaseFlitEvent : ClockedEvent, IDirectedEvent<IRoutable>
    {
        public Flit Flit { get; set; }

        public IRoutable CurrentSender { get; set; }
        public IRoutable CurrentReceiver { get; set; }

        public override int ClockCycle { get; set; }
    }

    public class FlitEvent : BaseFlitEvent
    {
    }

    public class FlitDeclinedEvent : BaseFlitEvent
    {
    }

    public class FlitAcceptedEvent : BaseFlitEvent
    {
    }
}