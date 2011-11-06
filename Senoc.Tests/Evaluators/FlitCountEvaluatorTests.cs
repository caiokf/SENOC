using Senoc.Model.Evaluators;

using NUnit.Framework;
using Senoc.Model.Eventing;
using Senoc.Model.Events;
using SharpTestsEx;
using StructureMap;

namespace Senoc.Tests.Evaluators
{
    [TestFixture]
    public class FlitCountEvaluatorTests : BaseTest
    {
        protected FlitCountEvaluator evaluator;

        [SetUp]
        public override void setup()
        {
            base.setup();
            evaluator = ObjectFactory.GetInstance<FlitCountEvaluator>();
        }

        [Test]
        public void should_count_flit_events()
        {
            distributor.SendMessage(new FlitEvent { ClockCycle = 1 });
            distributor.Distribute(1);

            evaluator.FlitsSent.Should().Be.EqualTo(1);
            evaluator.FlitsAccepted.Should().Be.EqualTo(0);
            evaluator.FlitsDeclined.Should().Be.EqualTo(0);
        }

        [Test]
        public void should_count_flit_accepted_events()
        {
            distributor.SendMessage(new FlitAcceptedEvent { ClockCycle = 1 });
            distributor.Distribute(1);

            evaluator.FlitsSent.Should().Be.EqualTo(0);
            evaluator.FlitsAccepted.Should().Be.EqualTo(1);
            evaluator.FlitsDeclined.Should().Be.EqualTo(0);
        }

        [Test]
        public void should_count_flit_declined_events()
        {
            distributor.SendMessage(new FlitDeclinedEvent { ClockCycle = 1 });
            distributor.Distribute(1);

            evaluator.FlitsSent.Should().Be.EqualTo(0);
            evaluator.FlitsAccepted.Should().Be.EqualTo(0);
            evaluator.FlitsDeclined.Should().Be.EqualTo(1);
        }

        [Test]
        public void should_correlate_flit_events_in_one_count()
        {
            var flit = new Flit();
            distributor.SendMessage(new FlitEvent { ClockCycle = 1, Flit = flit });
            distributor.SendMessage(new FlitEvent { ClockCycle = 2, Flit = flit });
            distributor.Distribute(1, 2);

            evaluator.FlitsSent.Should().Be.EqualTo(1);
        }

        [Test]
        public void should_correlate_flit_accepted_events_in_one_count()
        {
            var flit = new Flit();
            distributor.SendMessage(new FlitAcceptedEvent { ClockCycle = 1, Flit = flit });
            distributor.SendMessage(new FlitAcceptedEvent { ClockCycle = 2, Flit = flit });
            distributor.Distribute(1, 2);

            evaluator.FlitsAccepted.Should().Be.EqualTo(1);
        }

        [Test]
        public void should_correlate_flit_declined_events_in_one_count()
        {
            var flit = new Flit();
            distributor.SendMessage(new FlitDeclinedEvent { ClockCycle = 1, Flit = flit });
            distributor.SendMessage(new FlitDeclinedEvent { ClockCycle = 2, Flit = flit });
            distributor.Distribute(1, 2);

            evaluator.FlitsDeclined.Should().Be.EqualTo(1);
        }
    }
}
