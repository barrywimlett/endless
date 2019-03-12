using Next.Search.DataRefresh.Messages;
using Next.Search.Messaging.Rabbit;
using Next.Search.Messaging.Topology.Interfaces;

namespace Next.Search.Messaging.Topology
{
    public class PublishedXmlEndpoint : SimpleEndpoint<PublishedXmlMessage>, IPublishedXmlEndpoint, IPublishedXmlSender
    {
        public PublishedXmlEndpoint(IRabbitMessagingSettings settings)
            : base(settings)
        {
        }
    }
}