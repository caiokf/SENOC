using Senoc.Model.Eventing;

namespace Senoc.Model.Routers
{
    public class Channel : IChannel 
    {
        private Flit reservedFor;
        private readonly IBuffer inputBuffer;

        public Channel(IBuffer inputBuffer)
        {
            this.inputBuffer = inputBuffer;
        }

        public bool IsFull()
        {
            return inputBuffer.IsFull();
        }

        public bool IsReservedForOtherThan(Flit flit)
        {
            return (reservedFor != null) && (reservedFor != flit);
        }

        public void Reserve(Flit flit)
        {
            reservedFor = flit;
        }

        public void Release(Flit flit)
        {
            reservedFor = null;
        }
    }
}