using Next.Search.DataRefresh.Messages;
using Next.Search.Messaging.Rabbit;
using Next.Search.Messaging.Topology.Interfaces;

namespace Next.Search.Messaging.Topology
{
    public class JobExecuteEndpoint : SimpleEndpoint<JobExecuteMessage>, IJobExecuteEndpoint
    {
        public JobExecuteEndpoint(IRabbitMessagingSettings settings)
            : base(settings)
        {
        }
    }
}