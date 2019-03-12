using Next.Search.DataRefresh.Messages;
using Next.Search.Messaging.Generic;

namespace Next.Search.Messaging.Topology.Interfaces
{
    public interface IPriceStatusSender : ISender<PriceMessage>
    {
    }

    public interface IPriceStatusListener : IListener<PriceMessage>
    {
    }

    public interface IPriceStatusEndpoint : IEndpoint<PriceMessage>, IPriceStatusListener, IPriceStatusSender
    {
    }
}