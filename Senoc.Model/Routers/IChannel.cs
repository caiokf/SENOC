using Senoc.Model.Eventing;

namespace Senoc.Model.Routers
{
    public interface IChannel
    {
        bool IsFull();
        bool IsReservedForOtherThan(Flit flit);
        void Reserve(Flit flit);
        void Release(Flit flit);
    }
}