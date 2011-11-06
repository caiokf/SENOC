using System.Collections.Concurrent;
using System.Linq;
using Senoc.Model.Eventing;
using Senoc.Model.Events;

namespace Senoc.Model.Evaluators
{
    public class HopCountEvaluator : IEvaluateMetrics,
        IListenTo<FlitAcceptedEvent>.All
    {
        private readonly ConcurrentDictionary<Flit, int> hopTracking;

        public HopCountEvaluator()
        {
            hopTracking = new ConcurrentDictionary<Flit, int>();
        }

        public void Handle(FlitAcceptedEvent message)
        {
            var messageFlit = message.Flit ?? new Flit();
            hopTracking.AddOrUpdate(messageFlit, 1, (flit, value) => value + 1);
        }

        public double HopCount
        {
            get { return hopTracking.Average(x => x.Value); }
        }
    }
}
