using System;
using System.Diagnostics;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Endless.Messaging.Rabbit
{
    abstract public class AsyncConsumer<TEventArgs> : DefaultBasicConsumer where TEventArgs : BasicDeliverEventArgs
    {
        protected AsyncConsumer(Func<TEventArgs, Task> onMessageRecieved)
        {
            this.OnMessageRecieved = onMessageRecieved;
        }

        protected AsyncConsumer(IModel model) : base(model)
        {
        }

        Func<TEventArgs, Task> OnMessageRecieved;

        protected abstract TEventArgs CreateEventArgs(string consumerTag, ulong deliveryTag, bool redelivered,
            string exchange, string routingKey,
            IBasicProperties properties, byte[] body);

        override public void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered,
            string exchange, string routingKey,
            IBasicProperties properties, byte[] body)
        {
            try
            {
                var args = CreateEventArgs(consumerTag, deliveryTag, redelivered, exchange, routingKey, properties,
                    body);

                base.HandleBasicDeliver(consumerTag, deliveryTag, redelivered, exchange, routingKey, properties,
                    body);
                Task t = OnMessageRecieved(args);

                t.Wait();
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Message with deliveryTag {deliveryTag} NACKED, requeued");
                this.Model.BasicNack(deliveryTag, false, true);
            }
        }
    }
}