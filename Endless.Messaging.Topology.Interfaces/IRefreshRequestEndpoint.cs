using Next.Search.DataRefresh.Messages;
using Next.Search.Messaging.Generic;

namespace Next.Search.Messaging.Topology.Interfaces
{
    public interface IRefreshRequestSender : ISender<RefreshRequestMessage>
    {
    }

    public interface IRefreshRequestListener : IListener<RefreshRequestMessage>
    {
    }

    public interface IRefreshRequestEndpoint : IEndpoint<RefreshRequestMessage>, IRefreshRequestListener, IRefreshRequestSender
    {
    }
}
