using Next.Search.DataRefresh.Messages;
using Next.Search.Messaging.Generic;

namespace Next.Search.Messaging.Topology.Interfaces
{
    public interface IJobSender : ISender<JobMessage>
    {
    }

    public interface IJobListener : IListener<JobMessage>
    {
    }

    public interface IJobEndpoint : IEndpoint<JobMessage>, IJobListener, IJobSender
    {
    }
}