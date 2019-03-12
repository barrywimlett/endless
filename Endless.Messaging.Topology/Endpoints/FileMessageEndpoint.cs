using Next.Search.DataRefresh.Messages;
using Next.Search.Messaging.Rabbit;
using Next.Search.Messaging.Topology.Interfaces;

namespace Next.Search.Messaging.Topology
{   
    public class FileMessageEndpoint : SimpleEndpoint<FileMessage>, IFileMessageEndpoint
    {
        public FileMessageEndpoint(IRabbitMessagingSettings settings)
            : base(settings)
        {
        }
    }
}