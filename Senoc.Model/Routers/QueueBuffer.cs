using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Senoc.Model.Eventing;

namespace Senoc.Model.Routers
{
    public class QueueBuffer : IBuffer
    {
        private readonly int bufferDepth;

        protected ConcurrentQueue<Flit> items = new ConcurrentQueue<Flit>();

        public IEnumerable<Flit> Items { get { return items.AsEnumerable(); } }

        public QueueBuffer()
        {
                
        }

        public QueueBuffer(int bufferDepth)
        {
            if (bufferDepth <= 0)
                throw new ArgumentException("Invalid buffer depth: " + bufferDepth);

            this.bufferDepth = bufferDepth;
        }

        public virtual bool IsFull()
        {
            return items.Count >= bufferDepth;
        }
        
        public virtual bool IsEmpty()
        {
           return items.Count == 0;
        }
        
        public virtual void Put(Flit flit)
        {
            if (items.Count < bufferDepth)
                items.Enqueue(flit);
        }

        public virtual Flit Take()
        {
            if (IsEmpty())
                return null;
            
            Flit flit;
            items.TryDequeue(out flit);
            return flit;
        }

        public virtual Flit Peek()
        {
            if (IsEmpty())
                return null;
            
            Flit flit;
            items.TryPeek(out flit);
            return flit;
        }     
    }
}