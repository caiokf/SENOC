using Senoc.Model.Eventing;

namespace Senoc.Model.Events
{
    public class ClockedEvent : IEvent
    {
        public virtual int ClockCycle { get; set; }
    }
}