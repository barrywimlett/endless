using Next.Search.DataRefresh.Messages;
using Next.Search.Messaging.Generic;

namespace Next.Search.Messaging.Topology.Interfaces
{
    public interface INewInStatusSender : ISender<NewInStatusMessage>
    {
    }

    public interface INewInStatusListener : IListener<NewInStatusMessage>
    {
    }

    public interface INewInStatusEndpoint : IEndpoint<NewInStatusMessage>, INewInStatusListener, INewInStatusSender
    {
    }
}