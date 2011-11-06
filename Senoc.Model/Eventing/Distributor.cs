using System;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Senoc.Model.Eventing
{
    public class Distributor : IDistributor
    {
        private readonly SynchronizationContext _context;
        private readonly List<object> _listeners = new List<object>();
        private readonly object _locker = new object();

        private readonly ConcurrentBag<IEvent> _distributionItems;

        public Distributor(SynchronizationContext context)
        {
            _context = context;
            _distributionItems = new ConcurrentBag<IEvent>();
        }
        
        public void SendMessage<T>(T message) where T : IEvent
        {
            _distributionItems.Add(message);
        }
        
        public void Distribute(int fromCycle, int toCycle)
        {
            for (var i = fromCycle; i <= toCycle; i++)
                Distribute(i);
        }

        public void Distribute(int clockCycle)
        {
            var messagesToDistribute = _distributionItems
                .Where(x => x.ClockCycle == clockCycle).ToList();

            messagesToDistribute.ForEach(message =>
            {
                SendAction(() => All().AsParallel().ForAll(listener =>
                {
                    ((IListenTo)listener).Handle(message, message.GetType());
                }));
            });

            _distributionItems.ToList().RemoveAll(x => x.ClockCycle == clockCycle);
        }

        public void AddListener(object listener)
        {
            WithinLock(() =>
            {
                if (_listeners.Contains(listener)) 
                    return;

                _listeners.Add(listener);
            });
        }

        public void RemoveListener(object listener)
        {
            WithinLock(() => _listeners.Remove(listener));
        }
        

        private IEnumerable<object> All()
        {
            lock (_locker)
            {
                return _listeners;
            }
        }

        private void WithinLock(Action action)
        {
            lock (_locker)
            {
                action();
            }
        }

        private void SendAction(Action action)
        {
            _context.Send(state => action(), null);
        }
    }
}