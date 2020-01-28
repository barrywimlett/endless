using System.Diagnostics;
using System.Text;
using Endless.Messaging.Rabbit.Interfaces;
using Endless.Messaging.Rabbit.MessageFactories;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Endless.Messaging.Rabbit.Generic
{
    public class RabbitSender<TMessageBody> : RabbitConnection,IRabbitSender<TMessageBody>
        where TMessageBody : class, new()
    {
        private readonly IRabbitMessagingSettings _messagingSettings;
        private readonly IBasicProperties _properties;
        private readonly string _queueName;

        

        public RabbitSender(IRabbitMessagingSettings messagingSettings, string queueName) :base(messagingSettings,false)
            
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

        

        
        public void SendBasicMessage(IBasicRabbitMessage<TMessageBody> message)
        {
            _channel.BasicPublish(string.Empty, _queueName, message.BasicProperties, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message.MessageBody)));
        }

        public IMessageEnvelope<TMessageBody> GetEnvelope(TMessageBody body)
        {
            return new BasicRabbitMessage<TMessageBody>(body, _channel.CreateBasicProperties());
        }

        public void Send(IMessageEnvelope<TMessageBody> message)
        {
            var basicMessage = message as IBasicRabbitMessage<TMessageBody>;
            if (basicMessage == null)
            {
                Debug.Assert(false);
            }
            else
            {
                SendBasicMessage(basicMessage);
            }
        }
    }

    
}