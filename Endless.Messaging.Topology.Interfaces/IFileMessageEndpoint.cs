using Next.Search.DataRefresh.Messages;
using Next.Search.Messaging.Generic;

namespace Next.Search.Messaging.Topology.Interfaces
{
    public interface IFileMessageSender : ISender<FileMessage>
    {
    }

    public interface IFileMessageListener : IListener<FileMessage>
    {
    }

    public interface IFileMessageEndpoint : IEndpoint<FileMessage>, IFileMessageSender, IFileMessageListener
    {
    }
}
