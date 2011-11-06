using System.Collections.Generic;
using Senoc.Model.Eventing;
using Senoc.Model.RoutingAlgorithms;

namespace Senoc.Model.Routers
{
    public class SwitchingMatrix : ISwitchingMatrix
    {
        private readonly IRoutingAlgorithm routingAlgorithm;
        private readonly List<IChannel> channels = new List<IChannel>();

        public SwitchingMatrix(IRoutingAlgorithm routingAlgorithm)
        {
            this.routingAlgorithm = routingAlgorithm;
        }

        public List<IChannel> Channels
        {
            get { return channels; }
        }

        public void AddChannel(IChannel channel)
        {
            channels.Add(channel);
        }

        public RoutingAvailabilityExpression HasAvailableChannelFor(Flit flit)
        {
            var channel = routingAlgorithm.Route(Channels);
            var isAvailable = (!channel.IsFull() && !channel.IsReservedForOtherThan(flit));

            return new RoutingAvailabilityExpression(
                isAvailable: isAvailable, 
                routeIfAvailableAction: () => { if (isAvailable) this.Route(flit, channel); }
            );
        }

        private void Route(Flit flit, IChannel channel)
        {
            if (flit.Type == FlitType.Header) channel.Reserve(flit);
            if (flit.Type == FlitType.Tail) channel.Release(flit);
        }
    }
}