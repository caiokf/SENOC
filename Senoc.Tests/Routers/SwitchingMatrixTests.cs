using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Senoc.Model.Eventing;
using Senoc.Model.Routers;
using Senoc.Model.RoutingAlgorithms;
using SharpTestsEx;

namespace Senoc.Tests.Routers
{
    [TestFixture]
    public class SwitchingMatrixTests : BaseTest
    {
        [Test]
        public void after_add_channel_should_have_it_on_its_list()
        {
            var channel = MockRepository.GenerateStub<IChannel>();
            var routingAlgorithm = MockRepository.GenerateStub<IRoutingAlgorithm>();
            var switchingMatrix = new SwitchingMatrix(routingAlgorithm);
            
            switchingMatrix.AddChannel(channel);

            switchingMatrix.Channels.Should().Have.SameSequenceAs(new[] {channel});
        }

        [Test]
        public void should_ask_for_routing_algorithm_when_verifying_channels_availability()
        {
            var routingAlgorithm = MockRepository.GenerateMock<IRoutingAlgorithm>();
            var switchingMatrix = new SwitchingMatrix(routingAlgorithm);
            switchingMatrix.AddChannel(MockRepository.GenerateStub<IChannel>());

            routingAlgorithm.Expect(x => x.Route(switchingMatrix.Channels))
                .Return(switchingMatrix.Channels.FirstOrDefault());

            switchingMatrix.HasAvailableChannelFor(new Flit());
            
            routingAlgorithm.VerifyAllExpectations();
        }

        [Test]
        public void should_have_channel_for_flit_when_buffers_empty_and_not_reserved()
        {
            var returningChannel = MockRepository.GenerateMock<IChannel>();
            var routingAlgorithm = MockRepository.GenerateStub<IRoutingAlgorithm>();
            var flit = new Flit();

            returningChannel.Expect(x => x.IsFull()).Return(false);
            returningChannel.Expect(x => x.IsReservedForOtherThan(flit)).Return(false);
            routingAlgorithm.Stub(x => x.Route(null)).IgnoreArguments().Return(returningChannel);

            var switchingMatrix = new SwitchingMatrix(routingAlgorithm);
            switchingMatrix.AddChannel(returningChannel);

            switchingMatrix.HasAvailableChannelFor(flit).IsAvailable.Should().Be.True();
            returningChannel.VerifyAllExpectations();
        }

        [Test]
        public void should_not_have_channel_for_flit_when_buffers_full()
        {
            var returningChannel = MockRepository.GenerateMock<IChannel>();
            var routingAlgorithm = MockRepository.GenerateStub<IRoutingAlgorithm>();
            var flit = new Flit();

            returningChannel.Expect(x => x.IsFull()).Return(true);
            returningChannel.Expect(x => x.IsReservedForOtherThan(flit)).Return(false);
            routingAlgorithm.Stub(x => x.Route(null)).IgnoreArguments().Return(returningChannel);
            
            var switchingMatrix = new SwitchingMatrix(routingAlgorithm);
            switchingMatrix.AddChannel(returningChannel);

            switchingMatrix.HasAvailableChannelFor(flit).IsAvailable.Should().Be.False();
        }

        [Test]
        public void should_not_have_channel_for_flit_when_channel_reserved()
        {
            var returningChannel = MockRepository.GenerateMock<IChannel>();
            var routingAlgorithm = MockRepository.GenerateStub<IRoutingAlgorithm>();
            var flit = new Flit();

            returningChannel.Expect(x => x.IsFull()).Return(false);
            returningChannel.Expect(x => x.IsReservedForOtherThan(flit)).Return(true);
            routingAlgorithm.Stub(x => x.Route(null)).IgnoreArguments().Return(returningChannel);

            var switchingMatrix = new SwitchingMatrix(routingAlgorithm);
            switchingMatrix.AddChannel(returningChannel);

            switchingMatrix.HasAvailableChannelFor(flit).IsAvailable.Should().Be.False();
            returningChannel.VerifyAllExpectations();
        }

        [Test]
        public void should_reserve_channel_for_flit_when_the_head_pass_by()
        {
            var returningChannel = MockRepository.GenerateMock<IChannel>();
            var routingAlgorithm = MockRepository.GenerateStub<IRoutingAlgorithm>();
            var flit = new Flit { Type = FlitType.Header };

            returningChannel.Stub(x => x.IsFull()).Return(false);
            returningChannel.Stub(x => x.IsReservedForOtherThan(flit)).Return(false);
            routingAlgorithm.Stub(x => x.Route(null)).IgnoreArguments().Return(returningChannel);

            var switchingMatrix = new SwitchingMatrix(routingAlgorithm);
            switchingMatrix.AddChannel(returningChannel);

            returningChannel.Expect(x => x.Reserve(flit));
            switchingMatrix.HasAvailableChannelFor(flit).RouteIfAvailable();

            returningChannel.VerifyAllExpectations();
        }

        [Test]
        public void should_release_channel_for_flit_when_tail_pass_by()
        {
            var returningChannel = MockRepository.GenerateMock<IChannel>();
            var routingAlgorithm = MockRepository.GenerateStub<IRoutingAlgorithm>();
            var flit = new Flit { Type = FlitType.Tail };

            returningChannel.Stub(x => x.IsFull()).Return(false);
            returningChannel.Stub(x => x.IsReservedForOtherThan(flit)).Return(false);
            routingAlgorithm.Stub(x => x.Route(null)).IgnoreArguments().Return(returningChannel);

            var switchingMatrix = new SwitchingMatrix(routingAlgorithm);
            switchingMatrix.AddChannel(returningChannel);

            returningChannel.Expect(x => x.Release(flit));
            switchingMatrix.HasAvailableChannelFor(flit).RouteIfAvailable();

            returningChannel.VerifyAllExpectations();
        }

        [Test]
        [Ignore]
        public void still_tests_missing()
        {
            Assert.Fail();
        }
        /*
         * 
         * AVAILABLE (channel_out)
         * return channel_out is not reserved
         * 
         * 
         * RESERVE (channel_in, channel_out)
         * if (channel_out available)
         *      channel_out <= (reserved for channel_in)
         * 
         * 
         * RELEASE (channel_out)
         * channel_out <= (not reserved)
         * 
         * 
         * GET OUTPUT PORT (channel_in)
         * return channel_out reserved for channel_in OR none
         * 
         * 
         * INVALIDATE (channel_out)
         * makes port no longer available for reservation
         * 
         */
    }
}