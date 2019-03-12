using Next.Search.DataRefresh.Messages;
using Next.Search.Messaging.Rabbit;
using Next.Search.Messaging.Topology.Interfaces;

namespace Next.Search.Messaging.Topology
{
    public class StockStatusEndpoint : SimpleEndpoint<StockMessage>, IStockStatusEndpoint
    {
        public StockStatusEndpoint(IRabbitMessagingSettings settings)
            : base(settings)
        {
        }
    }
}