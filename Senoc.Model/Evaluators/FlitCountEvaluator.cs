using System.Linq;
using System.Collections.Concurrent;
using Senoc.Model.Eventing;
using Senoc.Model.Events;

namespace Senoc.Model.Evaluators
{
    public class FlitCountEvaluator : IEvaluateMetrics,
                                      IListenTo<FlitEvent>.All,
                                      IListenTo<FlitAcceptedEvent>.All,
                                      IListenTo<FlitDeclinedEvent>.All
    {
        private readonly ConcurrentBag<Flit> flitsSent;
        private readonly ConcurrentBag<Flit> flitsAccepted;
        private readonly ConcurrentBag<Flit> flitsDeclined;

        public FlitCountEvaluator()
        {
            flitsSent = new ConcurrentBag<Flit>();
            flitsAccepted = new ConcurrentBag<Flit>();
            flitsDeclined = new ConcurrentBag<Flit>();
        }

        public void Handle(FlitEvent message)
        {
            if (flitsSent.Count(x => x == message.Flit) == 0)
                flitsSent.Add(message.Flit);
        }

        public void Handle(FlitAcceptedEvent message)
        {
            if (flitsAccepted.Count(x => x == message.Flit) == 0) 
                flitsAccepted.Add(message.Flit);
        }

        public void Handle(FlitDeclinedEvent message)
        {
            if (flitsDeclined.Count(x => x == message.Flit) == 0)
                flitsDeclined.Add(message.Flit);    
        }

        public int FlitsSent { get { return flitsSent.Count; } }
        public int FlitsAccepted { get { return flitsAccepted.Count; } }
        public int FlitsDeclined { get { return flitsDeclined.Count; } }

    }
}