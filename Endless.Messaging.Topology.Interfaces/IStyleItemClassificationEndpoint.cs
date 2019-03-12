using Next.Search.DataRefresh.Messages;
using Next.Search.Messaging.Generic;

namespace Next.Search.Messaging.Topology.Interfaces
{
    public interface IStyleItemClassificationSender : ISender<StyleItemClassificationMessage>
    {
    }

    public interface IStyleItemClassificationListener : IListener<StyleItemClassificationMessage>
    {
    }

    public interface IStyleItemClassificationEndpoint : IEndpoint<StyleItemClassificationMessage>, IStyleItemClassificationListener, IStyleItemClassificationSender
    {
    }
}