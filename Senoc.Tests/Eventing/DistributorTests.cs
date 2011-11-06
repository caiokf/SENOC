using System;
using System.Threading;
using NUnit.Framework;
using Rhino.Mocks;
using Senoc.Model.Eventing;
using Senoc.Model.Events;

namespace Senoc.Tests.Eventing
{
    [TestFixture]
    public class DistributorTests : BaseTest
    {
        [Test]
        public void should_not_send_clock_2_to_noone_before_all_clock_1_being_sent()
        {
            Action<MethodInvocation> afterHandleAction = x => { };
            OrderedExpectationsForListeners(afterHandleAction);
        }

        [Test]
        public void should_not_send_clock_2_to_noone_before_all_clock_1_being_sent_with_delay()
        {
            Action<MethodInvocation> afterHandleAction = x => Thread.Sleep(10);
            OrderedExpectationsForListeners(afterHandleAction);
        }
        
        private void OrderedExpectationsForListeners(Action<MethodInvocation> afterHandleAction)
        {
            var mockRepository = new MockRepository();
            var mockA = mockRepository.StrictMock<IListenTo<ClockedEvent>.All>();
            var mockB = mockRepository.StrictMock<IListenTo<ClockedEvent>.All>();
            distributor.AddListener(mockA);
            distributor.AddListener(mockB);

            var clockedEvent1 = new ClockedEvent { ClockCycle = 1 };
            var clockedEvent2 = new ClockedEvent { ClockCycle = 2 };

            using (mockRepository.Ordered())
            {
                using (mockRepository.Unordered())
                {
                    Expect.Call(() => mockA.Handle(clockedEvent1)).WhenCalled(afterHandleAction);
                    Expect.Call(() => mockB.Handle(clockedEvent1)).WhenCalled(afterHandleAction);
                }
                using (mockRepository.Unordered())
                {
                    Expect.Call(() => mockA.Handle(clockedEvent2)).WhenCalled(afterHandleAction);
                    Expect.Call(() => mockB.Handle(clockedEvent2)).WhenCalled(afterHandleAction);
                }
            }

            mockRepository.ReplayAll();

            distributor.SendMessage(clockedEvent1);
            distributor.SendMessage(clockedEvent2);
            distributor.Distribute(1);
            distributor.Distribute(2);

            Thread.Sleep(550);
            mockRepository.VerifyAll();
        }

        [Test]
        public void should_be_able_to_register_listener_and_send_message()
        {
            var mock = MockRepository.GenerateMock<IListenTo<ClockedEvent>.All>();
            distributor.AddListener(mock);

            var clockedEvent1 = new ClockedEvent { ClockCycle = 1 };

            mock.Expect(x => x.Handle(clockedEvent1));

            distributor.SendMessage(clockedEvent1);
            distributor.Distribute(1);

            mock.VerifyAllExpectations();
        }

        [Test]
        public void should_be_able_to_register_filtered_listeners_and_send_message()
        {
            var mockToAccept = MockRepository.GenerateMock<IListenTo<ClockedEvent>.ThatSatisfy>();
            var mockToFilterOut = MockRepository.GenerateMock<IListenTo<ClockedEvent>.ThatSatisfy>();
            distributor.AddListener(mockToAccept);
            distributor.AddListener(mockToFilterOut);

            var clockedEvent1 = new ClockedEvent { ClockCycle = 1 };

            mockToAccept.Expect(x => x.SatisfiedBy(clockedEvent1)).Return(true);
            mockToAccept.Expect(x => x.Handle(clockedEvent1));

            mockToFilterOut.Expect(x => x.SatisfiedBy(clockedEvent1)).Return(false);
            mockToFilterOut.Expect(x => x.Handle(clockedEvent1)).Repeat.Never();

            distributor.SendMessage(clockedEvent1);
            distributor.Distribute(1);

            mockToAccept.VerifyAllExpectations();
            mockToFilterOut.VerifyAllExpectations();
        }

        [Test]
        public void should_be_able_to_unregister_listener()
        {
            var mock = MockRepository.GenerateMock<IListenTo<ClockedEvent>.All>();
            distributor.AddListener(mock);

            var clockedEvent1 = new ClockedEvent { ClockCycle = 1 };
            mock.Expect(x => x.Handle(clockedEvent1));
            distributor.SendMessage(clockedEvent1);
            distributor.Distribute(1);
            
            distributor.RemoveListener(mock);
            var clockedEvent2 = new ClockedEvent { ClockCycle = 2 };
            mock.Expect(x => x.Handle(clockedEvent1)).Repeat.Never();
            distributor.SendMessage(clockedEvent2);
            distributor.Distribute(2);
            
            mock.VerifyAllExpectations();
        }
        
        [Test]
        [Ignore]
        public void all_events_should_be_fired_before_clock_event_for_a_given_cycle()
        {
            Assert.Fail();
        }

    }
}