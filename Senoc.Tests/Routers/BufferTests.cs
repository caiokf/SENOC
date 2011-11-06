using System;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using Senoc.Model.Eventing;
using Senoc.Model.Routers;
using SharpTestsEx;

namespace Senoc.Tests.Routers
{
    [TestFixture]
    public class QueueBufferTests
    {
        [Test]
        public void should_increase_count_after_adding_one_element()
        {
            var bufferDepth = 2;
            var queue = new QueueBuffer(bufferDepth: bufferDepth);
            var count = queue.Items.Count();

            queue.Put(new Flit());

            queue.Items.Count().Should().Be.GreaterThan(count);
        }

        [Test]
        public void should_be_initially_empty()
        {
            var bufferDepth = 2;
            var queue = new QueueBuffer(bufferDepth: bufferDepth);

            queue.Items.Count().Should().Be.EqualTo(0);
        }

        [Test]
        public void should_be_full_when_added_items_until_maximum_capacity()
        {
            var bufferDepth = 2;
            var queue = new QueueBuffer(bufferDepth: bufferDepth);

            queue.Put(new Flit());
            queue.Put(new Flit());

            queue.IsFull().Should().Be.True();
        }

        [Test]
        public void should_decrease_items_count_after_take_from_queue()
        {
            var bufferDepth = 2;
            var queue = new QueueBuffer(bufferDepth: bufferDepth);
            queue.Put(new Flit());

            var count = queue.Items.Count();
            queue.Take();

            queue.Items.Count().Should().Be.LessThan(count);
        }

        [Test]
        public void should_not_decrease_items_count_after_peek_from_queue()
        {
            var bufferDepth = 2;
            var queue = new QueueBuffer(bufferDepth: bufferDepth);
            queue.Put(new Flit());

            var count = queue.Items.Count();
            queue.Peek();

            queue.Items.Count().Should().Be.EqualTo(count);
        }

        [Test]
        public void should_not_allow_buffer_depth_less_or_equals_to_zero()
        {
            Assert.Throws(typeof(ArgumentException),
                () => new QueueBuffer(bufferDepth: 0));

            Assert.Throws(typeof(ArgumentException),
                () => new QueueBuffer(bufferDepth: -1));
        }

        [Test]
        [Ignore]
        public void should_never_be_empty_and_full_at_same_time()
        {
            Assert.Fail();
        }

        [Test]
        public void should_be_able_to_add_items_when_queue_is_empty()
        {
            var queue = new MockRepository().PartialMock<QueueBuffer>(1);
            
            queue.Expect(x => x.IsEmpty()).Return(false);
            queue.Expect(x => x.Put(null)).CallOriginalMethod(OriginalCallOptions.NoExpectation);

            queue.Put(new Flit());

            queue.Items.Count().Should().Be.EqualTo(1);
        }

        [Test]
        public void take_should_return_the_first_flit_put_on_queue()
        {
            var bufferDepth = 2;
            var queue = new QueueBuffer(bufferDepth: bufferDepth);
            var flit1 = new Flit();
            var flit2 = new Flit();

            queue.Put(flit1);
            queue.Put(flit2);

            var take = queue.Take();
            take.Should().Be.EqualTo(flit1);
            take.Should().Not.Be.EqualTo(flit2);
        }

        [Test]
        public void peek_should_return_the_first_flit_put_on_queue()
        {
            var bufferDepth = 2;
            var queue = new QueueBuffer(bufferDepth: bufferDepth);
            var flit1 = new Flit();
            var flit2 = new Flit();

            queue.Put(flit1);
            queue.Put(flit2);

            var peek = queue.Peek();
            peek.Should().Be.EqualTo(flit1);
            peek.Should().Not.Be.EqualTo(flit2);
        }

        [Test]
        public void take_should_return_null_when_buffer_empty()
        {
            var bufferDepth = 2;
            var queue = new QueueBuffer(bufferDepth: bufferDepth);

            queue.Take().Should().Be.Null();
        }

        [Test]
        public void peek_should_return_null_when_buffer_empty()
        {
            var bufferDepth = 2;
            var queue = new QueueBuffer(bufferDepth: bufferDepth);

            queue.Peek().Should().Be.Null();
        }

        [Test]
        public void take_should_not_decrease_items_count_if_buffer_empty()
        {
            var bufferDepth = 2;
            var queue = new QueueBuffer(bufferDepth: bufferDepth);
            var count = queue.Items.Count();

            queue.Take();

            queue.Items.Count().Should().Be.EqualTo(count);
        }

        [Test]
        public void should_not_allow_add_more_items_than_buffer_depth()
        {
            var bufferDepth = 2;
            var queue = new QueueBuffer(bufferDepth: bufferDepth);
            queue.Put(new Flit());
            queue.Put(new Flit());
            queue.Put(new Flit());

            queue.IsFull().Should().Be.True();
            queue.Items.Count().Should().Be.EqualTo(bufferDepth);
        }
    }

    [TestFixture]
    public class InfinitBufferTests
    {
        [Test]
        public void should_never_be_full()
        {
            var buffer = new InfinitBuffer();
            buffer.IsFull().Should().Be.False();

            buffer.Put(new Flit());
            buffer.IsFull().Should().Be.False();

            buffer.Put(new Flit());
            buffer.IsFull().Should().Be.False();

            buffer.Put(new Flit());
            buffer.IsFull().Should().Be.False();
        }
    }
}