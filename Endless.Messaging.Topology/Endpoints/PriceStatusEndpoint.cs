using Next.Search.DataRefresh.Messages;
using Next.Search.Messaging.Rabbit;
using Next.Search.Messaging.Topology.Interfaces;

namespace Next.Search.Messaging.Topology
{
    public class PriceStatusEndpoint : SimpleEndpoint<PriceMessage>, IPriceStatusEndpoint, IPriceStatusSender
    {
        public PriceStatusEndpoint(IRabbitMessagingSettings settings)
            : base(settings)
        {
        }
    }
}