using Endless.Messaging.Rabbit.Interfaces;
using RabbitMQ.Client;

namespace Endless.Messaging.Rabbit
{
    // TODO: should we implement IDisposable to close connection/channels/queues ?
    public class RabbitBase
    {
        internal readonly IConnection _connection;
        protected IModel _channel;
        

        protected ISerializer _serializer = new JasonSerializer();

        public RabbitBase(IRabbitMessagingSettings messageSettings)
        {
            
        }

        public RabbitBase(IRabbitMessagingSettings messageSettings, string queueName)
            : this(messageSettings)
        {
            // are we asking for an exclusive queue
            if (string.IsNullOrEmpty(queueName))
            {
                var declareResult = _channel.QueueDeclare(autoDelete: true, exclusive: true);
                QueueName = declareResult.QueueName;
            }
            else
            {
                _channel.QueueDeclare(queueName, true, false, false, null);

                // TODO: need to consider other types of exchange
                _channel.ExchangeDeclare(queueName, "direct", true, false, null);
                _channel.QueueBind(queueName, queueName, queueName, null);
                QueueName = queueName;
            }
        }

        public string QueueName { get; protected set; }

        public IModel Channel
        {
            get { return _channel; }
        }

    }
}
