using System;

namespace Senoc.Model.Routers
{
    public class RoutingAvailabilityExpression
    {
        public bool IsAvailable { get; private set; }
        public Action RouteIfAvailable { get; private set; }

        public RoutingAvailabilityExpression(bool isAvailable)
        {
            this.IsAvailable = isAvailable;
            this.RouteIfAvailable = () => { };
        }

        public RoutingAvailabilityExpression(bool isAvailable, Action routeIfAvailableAction)
        {
            this.IsAvailable = isAvailable;
            this.RouteIfAvailable = routeIfAvailableAction;
        }
    }
}
