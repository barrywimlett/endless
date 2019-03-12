using System;
using Endless.Messaging.Rabbit.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Endless.Messaging.Rabbit.Generic
{
    public class
        ReplyingRabbitListener<TReceivingMessageBody, TReplyingWithMessageBody> : RabbitListener<
            TReceivingMessageBody>
        where TReceivingMessageBody : class, new()
        where TReplyingWithMessageBody : class, new()
    {
        private readonly Func<BasicDeliverEventArgs, TReplyingWithMessageBody> funcGenerateReply;

        public ReplyingRabbitListener(
            IRabbitMessagingSettings messagingSettings,
            string queueName,
            Func<BasicDeliverEventArgs, TReplyingWithMessageBody> funcGenerateReply)
            : base(messagingSettings, queueName)
        {
            this.funcGenerateReply = funcGenerateReply;
        }

        protected override void OnAcknowlegementSent(BasicDeliverEventArgs args)
        {
            var reply =
                new BasicRabbitMessage<TReplyingWithMessageBody>();

            reply.MessageBody = funcGenerateReply(args);

            reply.BasicProperties.CorrelationId = args.BasicProperties.CorrelationId;

            // var buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(reply.MessageBody));
            var buffer = _serializer.Serialize(reply.MessageBody);

            _channel.BasicPublish(string.Empty, args.BasicProperties.ReplyTo, reply.BasicProperties, buffer);
        }
    }
}