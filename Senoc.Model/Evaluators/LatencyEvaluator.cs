using System.Collections.Concurrent;
using System.Linq;
using Senoc.Model.Eventing;
using Senoc.Model.Events;

namespace Senoc.Model.Evaluators
{
    public class LatencyEvaluator : IListenTo<FlitAcceptedEvent>.All
    {
        private readonly ConcurrentDictionary<Flit, FlitTrack> latencies;

        public LatencyEvaluator()
        {
            latencies = new ConcurrentDictionary<Flit, FlitTrack>();
        }

        public double Latency
        {
            get
            {
                return latencies.Count == 0 ? 0 : 
                    latencies.Average(x => (x.Value.last - x.Value.first) + 1);
            }
        }
        
        public void Handle(FlitAcceptedEvent message)
        {
            latencies.AddOrUpdate(message.Flit, new FlitTrack {first = message.ClockCycle, last = message.ClockCycle},
                                  (flit, track) => new FlitTrack {first = track.first, last = message.ClockCycle});
        }

        private struct FlitTrack
        {
            public int first;
            public int last;
        }
    }
}