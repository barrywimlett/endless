using Next.Search.DataRefresh.Messages;
using Next.Search.Messaging.Rabbit;
using Next.Search.Messaging.Topology.Interfaces;

namespace Next.Search.Messaging.Topology
{
    public class JobCompleteEndpoint : SimpleEndpoint<DataRefreshMessage>, IJobCompleteEndpoint
    {
        public JobCompleteEndpoint(IRabbitMessagingSettings settings)
            : base(settings)
        {
        }
    }
}