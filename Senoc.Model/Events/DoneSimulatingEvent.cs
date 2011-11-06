using Senoc.Model.Primitives;

namespace Senoc.Model.Events
{
    public class DoneSimulatingEvent : ClockedEvent
    {
        public IRoutable DoneElement { get; set; }
    }
}