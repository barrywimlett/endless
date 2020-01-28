using Endless.Messaging.Rabbit.Generic;
using Endless.Messaging.Rabbit.Interfaces;
using Endless.Messaging.Rabbit.MessageFactories;
using RabbitMQ.Client;

namespace Endless.Messaging.Rabbit
{
    public class RabbitSender : RabbitConnection, IRabbitSender

    {
        private readonly IRabbitMessagingSettings _messagingSettings;
        private readonly IBasicProperties _properties;
        private readonly string _queueName;



        public RabbitSender(IRabbitMessagingSettings messagingSettings, string queueName) : base(messagingSettings, false)

        {
            //            _channel.QueueDeclare(queueName, true, false, false, null);

            // TODO: need to consider other types of exchange
            //          _channel.ExchangeDeclare(queueName, "direct", true, false, null);
            //        _channel.QueueBind(queueName, queueName, queueName, null);

            _messagingSettings = messagingSettings;
            _queueName = queueName;

            _properties = _channel.CreateBasicProperties();
            _properties.Persistent = true;
        }

        public void SendMessage(RabbitMessage message)
        {
            _channel.BasicPublish(string.Empty, _queueName, message.BasicProperties, message.Data);
        }

    }
}