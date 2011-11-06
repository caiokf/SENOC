using Senoc.Model.Eventing;

namespace Senoc.Model.Routers
{
    public class InfinitBuffer : QueueBuffer
    {
        public override bool IsFull()
        {
            return false;
        }

        public override void Put(Flit flit)
        {
            items.Enqueue(flit);
        }
    }
}