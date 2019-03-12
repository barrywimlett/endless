using RabbitMQ.Client.Events;

namespace Endless.Messaging.Rabbit.Generic
{
    /// <summary>
    /// Extends BasicDeliverEventArgs to include a typed MessageBody
    /// </summary>
    /// <typeparam name="TMessageBody"></typeparam>
    public class GenericDeliverEventArgs<TMessageBody> : BasicDeliverEventArgs
    {
        public GenericDeliverEventArgs(TMessageBody body, BasicDeliverEventArgs args)
        {
            ConsumerTag = args.ConsumerTag;
            DeliveryTag = args.DeliveryTag;
            Redelivered = args.Redelivered;
            Exchange = args.Exchange;
            RoutingKey = args.RoutingKey;
            BasicProperties = args.BasicProperties;
            Body = args.Body;

            MessageBody = body;
        }

        /// <summary>
        /// strongly-Typed MessageBody
        /// </summary>
        public TMessageBody MessageBody { get; }
    }
}