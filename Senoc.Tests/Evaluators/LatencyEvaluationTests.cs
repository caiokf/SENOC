using NUnit.Framework;
using Senoc.Model.Evaluators;
using Senoc.Model.Eventing;
using Senoc.Model.Events;
using SharpTestsEx;
using StructureMap;

namespace Senoc.Tests.Evaluators
{
    [TestFixture]
    public class LatencyEvaluationTests : BaseTest
    {
        protected LatencyEvaluator evaluator;

        [SetUp]
        public override void setup()
        {
            base.setup();
            evaluator = ObjectFactory.GetInstance<LatencyEvaluator>();
        }

        [Test]
        public void should_count_latency_on_accepted_flit_event()
        {
            distributor.SendMessage(new FlitAcceptedEvent { ClockCycle = 1, Flit = new Flit() });
            distributor.Distribute(1);

            evaluator.Latency.Should().Be.EqualTo(1);
        }

        [Test]
        public void should_not_count_latency_on_sent_and_decline_flit_event()
        {
            distributor.SendMessage(new FlitEvent { ClockCycle = 1, Flit = new Flit() });
            distributor.SendMessage(new FlitDeclinedEvent { ClockCycle = 1, Flit = new Flit() });
            distributor.Distribute(1);

            evaluator.Latency.Should().Be.EqualTo(0);
        }

        [Test]
        public void should_count_latency_for_separated_flits()
        {
            var firstFlit = new Flit();
            var secondFlit = new Flit();
            distributor.SendMessage(new FlitAcceptedEvent { ClockCycle = 1, Flit = firstFlit });
            distributor.SendMessage(new FlitAcceptedEvent { ClockCycle = 5, Flit = secondFlit });
            distributor.Distribute(1, 5);

            evaluator.Latency.Should().Be.EqualTo(1);
        }

        [Test]
        public void should_calculate_latency_from_first_to_last_time_flit_was_sent()
        {
            var flit = new Flit();
            distributor.SendMessage(new FlitAcceptedEvent { ClockCycle = 1, Flit = flit });
            distributor.SendMessage(new FlitAcceptedEvent { ClockCycle = 5, Flit = flit });
            distributor.Distribute(1, 5);

            evaluator.Latency.Should().Be.EqualTo(5);
        }

        [Test]
        public void should_calculate_average_latency_from_multiple_flits()
        {
            var firstFlit = new Flit();
            var secondFlit = new Flit();
            distributor.SendMessage(new FlitAcceptedEvent { ClockCycle = 1, Flit = firstFlit });
            distributor.SendMessage(new FlitAcceptedEvent { ClockCycle = 2, Flit = firstFlit });
            distributor.SendMessage(new FlitAcceptedEvent { ClockCycle = 5, Flit = secondFlit });
            distributor.Distribute(1, 5);

            evaluator.Latency.Should().Be.EqualTo(1.5);
        }
    }
}
