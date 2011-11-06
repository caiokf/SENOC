using System;
using Senoc.Model.Eventing;
using Senoc.Model.Events;
using Senoc.Model.Primitives;
using StructureMap;

namespace Senoc.Model.Routers
{
    public class Router : IRoutable
    {
        private readonly ISwitchingMatrix switchingMatrix;
        private readonly IDistributor distributor;

        public Router(ISwitchingMatrix switchingMatrix)
        {
            this.switchingMatrix = switchingMatrix;
            this.distributor = ObjectFactory.GetInstance<IDistributor>();
        }

        public int X  { get; set; }
        public int Y { get; set; }
        public Guid Id { get; set; }

        public void Clock(ClockEvent clockEvent)
        {
            throw new NotImplementedException();
        }

        public void Handle(SimulationStartedEvent message)
        {
            throw new NotImplementedException();
        }

        public void Handle(FlitEvent message)
        {
            var routingChannelExpression = switchingMatrix.HasAvailableChannelFor(message.Flit);

            if (routingChannelExpression.IsAvailable)
            {
                distributor.SendMessage(new FlitAcceptedEvent
                {
                    ClockCycle = message.ClockCycle + 1,
                    Flit = message.Flit
                });
            }
            else
            {
                distributor.SendMessage(new FlitDeclinedEvent
                {
                    ClockCycle = message.ClockCycle + 1,
                    Flit = message.Flit
                });
            }
        }

        public void Handle(FlitAcceptedEvent message)
        {
            throw new NotImplementedException();
        }

        public void Handle(FlitDeclinedEvent message)
        {
            throw new NotImplementedException();
        }
    }
}
