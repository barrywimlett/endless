using Next.Search.DataRefresh.Messages;
using Next.Search.Messaging.Rabbit;
using Next.Search.Messaging.Topology.Interfaces;

namespace Next.Search.Messaging.Topology
{
    public class RefreshCompleteEndpoint : SimpleEndpoint<RefreshCompleteMessage>, IRefreshCompleteEndpoint
    {
        public RefreshCompleteEndpoint(IRabbitMessagingSettings settings)
            : base(settings)
        {
        }
    }
}
