using Senoc.Model.Eventing;

namespace Senoc.Model.Routers
{
    public interface ISwitchingMatrix
    {
        RoutingAvailabilityExpression HasAvailableChannelFor(Flit flit);
    }
}
