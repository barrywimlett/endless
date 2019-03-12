using Next.Search.DataRefresh.Messages;
using Next.Search.Messaging.Rabbit;
using Next.Search.Messaging.Topology.Interfaces;

namespace Next.Search.Messaging.Topology
{
    public class JobMessageEndpoint : SimpleEndpoint<JobMessage>, IJobEndpoint
    {
        public JobMessageEndpoint(IRabbitMessagingSettings settings)
            : base(settings)
        {
        }
    }
}