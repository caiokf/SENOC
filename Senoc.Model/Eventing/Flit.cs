using System;
using Senoc.Model.Primitives;

namespace Senoc.Model.Eventing
{
    public class Flit : IIdentifiable
    {
        public Guid Id { get; set; }
        public int ClockCycle { get; set; }
        public FlitType Type { get; set; }
        public IRoutable CurrentSender { get; set; }
        public IRoutable CurrentReceiver { get; set; }
    }

    public enum FlitType
    {
        Header,
        Payload,
        Tail
    };
}