using Next.Search.DataRefresh.Messages;
using Next.Search.Messaging.Rabbit;
using Next.Search.Messaging.Topology.Interfaces;

namespace Next.Search.Messaging.Topology
{
    public class RefreshRequestEndpoint : SimpleEndpoint<RefreshRequestMessage>, IRefreshRequestEndpoint
    {
        public RefreshRequestEndpoint(IRabbitMessagingSettings settings)
            : base(settings)
        {
        }
    }
}
