using Next.Search.DataRefresh.Messages;
using Next.Search.Messaging.Generic;

namespace Next.Search.Messaging.Topology.Interfaces
{
    public interface IPublishedXmlSender : ISender<PublishedXmlMessage>
    {
    }

    public interface IPublishedXmlListener : IListener<PublishedXmlMessage>
    {
    }

    public interface IPublishedXmlEndpoint : IEndpoint<PublishedXmlMessage>, IPublishedXmlListener, IPublishedXmlSender
    {
    }
}