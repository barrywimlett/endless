using RabbitMQ.Client;

namespace Endless.Messaging.Rabbit
{
    public class ByteRecievedArgs : IByteRecievedArgs

    {
        public ByteRecievedArgs(
            string consumerTag,
            ulong deliveryTag,
            bool redelivered,
            string exchange,
            string routingKey,
            IBasicProperties properties,
            byte[] body)

        {
            this.ConsumerTag = consumerTag;
            this.DeliveryTag = deliveryTag;
            this.Redelivered = redelivered;
            this.Exchange = exchange;
            this.RoutingKey = routingKey;
            this.BasicProperties = properties;
            this.Body = body;
        }
        public ByteRecievedArgs(ByteRecievedArgs args) 
        {
            this.Body = args.Body;
            this.BasicProperties = args.BasicProperties;
            this.ConsumerTag = args.ConsumerTag;
            this.DeliveryTag = args.DeliveryTag;
            this.Exchange = args.Exchange;
            this.Redelivered = args.Redelivered;
            this.RoutingKey = RoutingKey;
        }

        /// <summary>The content header of the message.</summary>
        public IBasicProperties BasicProperties { get; protected set; }

        /// <summary>The message body.</summary>
        public byte[] Body { get; protected set; }

        /// <summary>The consumer tag of the consumer that the message
        /// was delivered to.</summary>
        public string ConsumerTag { get; protected set; }

        /// <summary>The delivery tag for this delivery. See
        /// IModel.BasicAck.</summary>
        public ulong DeliveryTag { get; protected set; }

        /// <summary>The exchange the message was originally published
        /// to.</summary>
        public string Exchange { get; protected set; }

        /// <summary>The AMQP "redelivered" flag.</summary>
        public bool Redelivered { get; protected set; }

        /// <summary>The routing key used when the message was
        /// originally published.</summary>
        public string RoutingKey { get; protected set; }
    }
}