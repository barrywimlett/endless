using Endless.Messaging.Rabbit.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;

namespace Endless.Messaging.Rabbit.Generic
{
    
    public class BasicRabbitMessage<TBody> : IBasicRabbitMessage, IBasicRabbitMessage<TBody>
        where TBody : class, new()
    {
        public BasicRabbitMessage()
        {
            BasicProperties = new BasicProperties();
            this.MessageBody = new TBody();
        }

        public BasicRabbitMessage(TBody messageBody)
            : this()
        {
            MessageBody = messageBody;
        }

        public BasicRabbitMessage(TBody messageBody, IBasicProperties basicProperties)
            : this()
        {
            MessageBody = messageBody;
            BasicProperties = basicProperties;
        }

        public BasicRabbitMessage(TBody messageBody, IModel channel)
            : this()
        {
            MessageBody = messageBody;
            BasicProperties = channel.CreateBasicProperties();
        }

        object IBasicRabbitMessage.MessageBody => MessageBody;

        public IBasicProperties BasicProperties { get; set; }

        public TBody MessageBody { get; set; }
    }
}