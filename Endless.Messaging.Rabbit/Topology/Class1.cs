using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endless.Messaging.Rabbit.Topology
{
    public interface IRoutingInformation
    {
        string ExchangeName { get; }
        string RoutingKey { get; }

    }

    public class DirectQueue : IRoutingInformation
    {
        public string QueueName { get; set; }

        string IRoutingInformation.ExchangeName => string.Empty;

        string IRoutingInformation.RoutingKey => QueueName;

    }
    public class BaseExchange : IRoutingInformation
    {
        public string ExchangeName { get; }
        public string RoutingKey { get; }

    }

    public class DirectExchange : BaseExchange 
    {

    }

    public class TopicExchange : BaseExchange
    {

    }

    public class Fanout : BaseExchange
    {

    }
}
