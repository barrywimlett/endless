using Next.Search.DataRefresh.Messages;
using Next.Search.Messaging.Rabbit;
using Next.Search.Messaging.Topology.Interfaces;

namespace Next.Search.Messaging.Topology
{
    public class NewInStatusEndpoint : SimpleEndpoint<NewInStatusMessage>, INewInStatusEndpoint
    {
        public NewInStatusEndpoint(IRabbitMessagingSettings settings)
            : base(settings)
        {
        }
    }
}