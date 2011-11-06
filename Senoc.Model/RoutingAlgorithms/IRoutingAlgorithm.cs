using System.Collections.Generic;
using Senoc.Model.Routers;

namespace Senoc.Model.RoutingAlgorithms
{
    public interface IRoutingAlgorithm
    {
        IChannel Route(List<IChannel> list);
    }
}