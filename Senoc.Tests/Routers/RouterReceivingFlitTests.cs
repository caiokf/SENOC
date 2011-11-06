using NUnit.Framework;
using Rhino.Mocks;
using Senoc.Model.Eventing;
using Senoc.Model.Events;
using Senoc.Model.Routers;

namespace Senoc.Tests.Routers
{
    [TestFixture]
    public class RouterReceivingFlitTests : BaseTest
    {
        [Test]
        public void should_verify_if_channels_are_available_when_receiving_flit()
        {
            var switchingMatrix = MockRepository.GenerateMock<ISwitchingMatrix>();
            var router = new Router(switchingMatrix);
            distributor.AddListener(router);
            var flit = new Flit();

            switchingMatrix.Expect(x => x.HasAvailableChannelFor(flit))
                .Return(new RoutingAvailabilityExpression(true));

            distributor.SendMessage(new FlitEvent { ClockCycle = 1, Flit = flit, CurrentReceiver = router });
            distributor.Distribute(1);

            switchingMatrix.VerifyAllExpectations();
        }

        [Test]
        public void should_accept_flit_when_it_has_channels_available()
        {
            var switchingMatrix = MockRepository.GenerateStub<ISwitchingMatrix>();
            var flitAcceptReceiver = MockRepository.GenerateMock<IListenTo<FlitAcceptedEvent>.All>();
            distributor.AddListener(flitAcceptReceiver);

            var router = new Router(switchingMatrix);
            distributor.AddListener(router);
            
            switchingMatrix.Stub(x => x.HasAvailableChannelFor(null)).IgnoreArguments()
                .Return(new RoutingAvailabilityExpression(true));

            flitAcceptReceiver.Expect(x => x.Handle(null)).IgnoreArguments();

            distributor.SendMessage(new FlitEvent { ClockCycle = 1, Flit = new Flit(), CurrentReceiver = router });
            distributor.Distribute(1, 2);

            flitAcceptReceiver.VerifyAllExpectations();
        }

        [Test]
        public void should_decline_flit_when_no_channel_available()
        {
            var switchingMatrix = MockRepository.GenerateStub<ISwitchingMatrix>();
            var flitDeclinedReceiver = MockRepository.GenerateMock<IListenTo<FlitDeclinedEvent>.All>();
            distributor.AddListener(flitDeclinedReceiver);

            var router = new Router(switchingMatrix);
            distributor.AddListener(router);

            switchingMatrix.Stub(x => x.HasAvailableChannelFor(null)).IgnoreArguments()
                .Return(new RoutingAvailabilityExpression(false));

            flitDeclinedReceiver.Expect(x => x.Handle(null)).IgnoreArguments();

            distributor.SendMessage(new FlitEvent {ClockCycle = 1, Flit = new Flit(), CurrentReceiver = router});
            distributor.Distribute(1, 2);

            flitDeclinedReceiver.VerifyAllExpectations();
        }

        [Test]
        [Ignore]
        public void should_put_flit_on_buffer_after_route_it()
        {
            Assert.Fail();
        }

        [Test]
        [Ignore]
        public void still_tests_missing()
        {
            Assert.Fail();
        }
        /*
         *
         * RECEIVE: obs circular buffer
         * 
         *  
            For each channel decide if a new flit can be accepted
            
            This process simply sees a flow of incoming flits. All arbitration
            and wormhole related issues are addressed in the txProcess()
         * 
         * foreach channel
         *      
         *      To accept a new flit, the following conditions must match:
	            
         *      1) there is an incoming request
	            2) there is a free slot in the input buffer of direction i
         * 
         *      if (is there a request AND channel.buffer is not full)
         *          buffer.put(flit)
         *          send ACK (Alternating Bit Protocol)
         *          power.Incoming()
         *      else
         *          send NACK
         *          power.stand_by()
         *      end if
         *      
         * end foreach
         * 
         * 
         * 
         */
    }
}
