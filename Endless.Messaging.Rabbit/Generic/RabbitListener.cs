using System;
using Endless.Messaging.Rabbit.Interfaces;
using RabbitMQ.Client.Events;

namespace Endless.Messaging.Rabbit.Generic
{
    /// <summary>
    /// strongly-typed listener for RabbitMQ messages
    /// </summary>
    /// <typeparam name="TMessageBody">Message</typeparam>
    /// <remarks>I really want to lose the "Generic" from the name</remarks>
    public class RabbitListener<TMessageBody> : RabbitListener, IListener<TMessageBody>
        where TMessageBody : class, new()
    {
        public RabbitListener(IRabbitMessagingSettings messagingSettings, string queueName)
            : base(messagingSettings, queueName)
        {
            funcProcessBytesRecieved = ProcessBytesRecieved;
        }

        public delegate void MessageReveivedEventHandler(IBasicRabbitMessage<TMessageBody> message, BasicDeliverEventArgs args);

        public event Action<IMessageEnvelope<TMessageBody>> MessageReceived;

        protected ISerializer _serializer = new JasonSerializer();

        protected virtual void OnMessageRecieved(BasicDeliverEventArgs args)
        {
            if (MessageReceived == null)
            {
            }
            else
            {
                var genericArgs = args as GenericDeliverEventArgs<TMessageBody>;
                if (genericArgs != null)
                {
                    var message = new BasicRabbitMessage<TMessageBody>(genericArgs.MessageBody, args.BasicProperties);
                    MessageReceived(message);
                }
            }
        }

        private void ProcessBytesRecieved(BasicDeliverEventArgs args)
        {
            var messageBody = _serializer.DeSerialize<TMessageBody>(args.Body);
            var message = new BasicRabbitMessage<TMessageBody>(messageBody, args.BasicProperties);

            var newArgs = new GenericDeliverEventArgs<TMessageBody>(messageBody, args);
            OnMessageRecieved(newArgs);
        }
    }
}