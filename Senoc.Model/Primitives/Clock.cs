using System;
using System.Collections.Generic;
using System.Threading;
using Senoc.Model.Eventing;

namespace Senoc.Model.Primitives
{
    public class Clock : IClock
    {
        private readonly List<IClockedElement> _subscribers;
        private readonly SynchronizationContext _context;
        private readonly IDistributor _distributor;
        private readonly object _locker;
        private int _clockCycle;
 
        public Clock(SynchronizationContext context, IDistributor distributor)
        {
            _context = context;
            _subscribers = new List<IClockedElement>();
            _distributor = distributor; 
            _locker = new object();
        }

        public void Cycle()
        {
            _distributor.Distribute(_clockCycle);

            var clockEvent = new ClockEvent { ClockCycle = _clockCycle };
            SendAction(() => All().ForEach(x => x.Clock(clockEvent)));
            
            _clockCycle++;
        }

        public void Cycle(int cycles)
        {
            for (var i = 0; i < cycles; i++)
                Cycle();
        }

        public void Subscribe(IClockedElement clockedElement)
        {
            WithinLock(() =>
            {
                if (_subscribers.Contains(clockedElement))
                    return;
                _subscribers.Add(clockedElement);
            });
        }

        private List<IClockedElement> All()
        {
            lock (_locker)
            {
                return _subscribers;
            }
        }

        private void WithinLock(Action action)
        {
            lock (_locker)
            {
                action();
            }
        }

        protected virtual void SendAction(Action action)
        {
            _context.Send(state => action(), null);
        }
    }
}