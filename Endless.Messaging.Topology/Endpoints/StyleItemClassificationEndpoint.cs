using Next.Search.DataRefresh.Messages;
using Next.Search.Messaging.Rabbit;
using Next.Search.Messaging.Topology.Interfaces;

namespace Next.Search.Messaging.Topology.Endpoints
{
    public class StyleItemClassificationEndpoint : SimpleEndpoint<StyleItemClassificationMessage>, IStyleItemClassificationEndpoint
    {
        public StyleItemClassificationEndpoint(IRabbitMessagingSettings settings)
            : base(settings)
        {
        }
    }
}