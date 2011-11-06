using NUnit.Framework;
using Rhino.Mocks;
using Senoc.Model.Eventing;
using Senoc.Model.Routers;
using SharpTestsEx;

namespace Senoc.Tests.Routers
{
    [TestFixture]
    public class ChannelTests : BaseTest
    {
        [Test]
        public void should_be_initially_empty_and_not_reserved()
        {
            var buffer = MockRepository.GenerateStub<IBuffer>();
            var channel = new Channel(buffer);

            channel.IsFull().Should().Be.False();
            channel.IsReservedForOtherThan(new Flit()).Should().Be.False();
        }

        [Test]
        public void should_be_marked_as_reserved_after_reservation()
        {
            var buffer = MockRepository.GenerateStub<IBuffer>();
            var channel = new Channel(buffer);
            var flit = new Flit();

            channel.Reserve(flit);

            channel.IsReservedForOtherThan(flit).Should().Be.False();
            channel.IsReservedForOtherThan(new Flit()).Should().Be.True();
        }

        [Test]
        public void should_be_marked_as_not_reserved_after_release()
        {
            var buffer = MockRepository.GenerateStub<IBuffer>();
            var channel = new Channel(buffer);
            var flit = new Flit();

            channel.Release(flit);

            channel.IsReservedForOtherThan(flit).Should().Be.False();
            channel.IsReservedForOtherThan(new Flit()).Should().Be.False();
        }

        [Test]
        public void should_be_marked_as_not_reserved_after_reserve_and_release()
        {
            var buffer = MockRepository.GenerateStub<IBuffer>();
            var channel = new Channel(buffer);
            var flit = new Flit();

            channel.Reserve(flit);
            channel.Release(flit);

            channel.IsReservedForOtherThan(flit).Should().Be.False();
            channel.IsReservedForOtherThan(new Flit()).Should().Be.False();
        }

        [Test]
        public void should_be_full_when_buffer_is_also_full()
        {
            var buffer = MockRepository.GenerateMock<IBuffer>();
            var channel = new Channel(buffer);

            buffer.Expect(x => x.IsFull()).Return(true);

            channel.IsFull().Should().Be.True();
            buffer.VerifyAllExpectations();
        }

        [Test]
        public void should_be_empty_when_buffer_is_also_empty()
        {
            var buffer = MockRepository.GenerateMock<IBuffer>();
            var channel = new Channel(buffer);

            buffer.Expect(x => x.IsFull()).Return(false);

            channel.IsFull().Should().Be.False();
            buffer.VerifyAllExpectations();
        }
    }
}