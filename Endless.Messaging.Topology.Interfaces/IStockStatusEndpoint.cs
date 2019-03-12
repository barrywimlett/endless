using Next.Search.DataRefresh.Messages;
using Next.Search.Messaging.Generic;

namespace Next.Search.Messaging.Topology.Interfaces
{
    public interface IStockStatusSender : ISender<StockMessage>
    {
    }

    public interface IStockStatusListener : IListener<StockMessage>
    {
    }

    public interface IStockStatusEndpoint : IEndpoint<StockMessage>, IStockStatusListener, IStockStatusSender
    {
    }
}