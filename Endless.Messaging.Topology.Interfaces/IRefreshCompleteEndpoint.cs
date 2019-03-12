using Next.Search.DataRefresh.Messages;
using Next.Search.Messaging.Generic;

namespace Next.Search.Messaging.Topology.Interfaces
{
    public interface IRefreshCompleteSender : ISender<RefreshCompleteMessage>
    {
    }

    public interface IRefreshCompleteListener : IListener<RefreshCompleteMessage>
    {
    }

    public interface IRefreshCompleteEndpoint : IEndpoint<RefreshCompleteMessage>, IRefreshCompleteListener, IRefreshCompleteSender
    {
    }
}
