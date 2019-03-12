using Next.Search.DataRefresh.Messages;
using Next.Search.Messaging.Generic;

namespace Next.Search.Messaging.Topology.Interfaces
{
    public interface IJobCompleteSender : ISender<DataRefreshMessage>
    {
    }

    public interface IJobCompleteListener : IListener<DataRefreshMessage>
    {
    }

    public interface IJobCompleteEndpoint : IEndpoint<DataRefreshMessage>, IJobCompleteListener, IJobCompleteSender
    {
    }
}