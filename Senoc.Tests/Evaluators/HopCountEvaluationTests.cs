using Senoc.Model.Evaluators;
using Senoc.Model.Eventing;
using Senoc.Model.Events;
using StructureMap;
using NUnit.Framework;
using SharpTestsEx;

namespace Senoc.Tests.Evaluators
{
    [TestFixture]
    public class HopCountEvaluationTests : BaseTest
    {
        protected HopCountEvaluator evaluator;

        [SetUp]
        public override void setup()
        {
            base.setup();
            evaluator = ObjectFactory.GetInstance<HopCountEvaluator>();
        }

        [Test]
        public void should_count_hops_on_accepted_flit_event()
        {
            distributor.SendMessage(new FlitAcceptedEvent{ ClockCycle = 1 });
            distributor.Distribute(1);

            evaluator.HopCount.Should().Be.EqualTo(1);
        }

        [Test]
        public void should_correlate_hops_for_same_flits()
        {
            var flit = new Flit();
            distributor.SendMessage(new FlitAcceptedEvent { ClockCycle = 1, Flit = flit});
            distributor.SendMessage(new FlitAcceptedEvent { ClockCycle = 2, Flit = flit });
            distributor.SendMessage(new FlitAcceptedEvent { ClockCycle = 3, Flit = flit });
            distributor.Distribute(1, 3);

            evaluator.HopCount.Should().Be.EqualTo(3);
        }

        [Test]
        public void should_calculate_average_hops_for_different_flits()
        {
            var firstFlit = new Flit();
            distributor.SendMessage(new FlitAcceptedEvent { ClockCycle = 1, Flit = firstFlit });
            distributor.SendMessage(new FlitAcceptedEvent { ClockCycle = 2, Flit = firstFlit });

            var secondFlit = new Flit();
            distributor.SendMessage(new FlitAcceptedEvent { ClockCycle = 3, Flit = secondFlit });

            distributor.Distribute(1, 3);

            evaluator.HopCount.Should().Be.EqualTo(1.5);
        }
    }
}
