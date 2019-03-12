using Next.Search.DataRefresh.Messages;
using Next.Search.Messaging.Generic;

namespace Next.Search.Messaging.Topology.Interfaces
{
    public interface IJobExecuteSender : ISender<JobExecuteMessage>
    {
    }

    public interface IJobExecuteListener : IListener<JobExecuteMessage>
    {
    }

    public interface IJobExecuteEndpoint : IEndpoint<JobExecuteMessage>, IJobExecuteListener, IJobExecuteSender
    {
    }
}