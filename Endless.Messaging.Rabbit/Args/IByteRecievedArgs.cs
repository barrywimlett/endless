﻿using RabbitMQ.Client;

namespace Endless.Messaging.Rabbit
{
    public interface IByteRecievedArgs

    {
        IBasicProperties BasicProperties { get; }


        /// <summary>The message body.</summary>

        byte[] Body { get; }


        /// <summary>The consumer tag of the consumer that the message
        /// was delivered to.</summary>

        string ConsumerTag { get; }


        /// <summary>The delivery tag for this delivery. See
        /// IModel.BasicAck.</summary>

        ulong DeliveryTag { get; }


        /// <summary>The exchange the message was originally published
        /// to.</summary>

        string Exchange { get; }


        /// <summary>The AMQP "redelivered" flag.</summary>

        bool Redelivered { get; }


        /// <summary>The routing key used when the message was
        /// originally published.</summary>

        string RoutingKey { get; }
    }
}