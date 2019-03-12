using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Endless.Messaging.Rabbit.Generic;
using Endless.Messaging.Rabbit.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Endless.Messaging.Rabbit
{
    public class RabbitListener : IRabbitListener
    {
        protected Action<BasicDeliverEventArgs> funcProcessBytesRecieved;
        private readonly Dictionary<string, RabbitConsumer> _consumers = new Dictionary<string, RabbitConsumer>();
        private RabbitConsumer _defaultConsumer;

        internal readonly IConnection _connection;
        protected IModel _channel;
        public string QueueName { get; protected set; }

        private bool _dispatchConsumersAsync = false;

        public RabbitListener(
            IRabbitMessagingSettings messagingSettings,
            string queueName,
            Action<BasicDeliverEventArgs> funcProcessBytesRecieved)
            : this(messagingSettings, queueName)
        {
            this.funcProcessBytesRecieved = funcProcessBytesRecieved;
        }

        public RabbitListener(IRabbitMessagingSettings messageSettings, string queueName)

        {
            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = messageSettings.HostName,
                RequestedHeartbeat = messageSettings.HeartbeatTimeout,
                AutomaticRecoveryEnabled = true,
                HandshakeContinuationTimeout = new System.TimeSpan(0, messageSettings.HandshakeTimeout, 0),
                NetworkRecoveryInterval = new System.TimeSpan(0, messageSettings.NetworkRecoveryInterval, 0),
                UserName = messageSettings.Username,
                Password = messageSettings.Password,

                DispatchConsumersAsync = _dispatchConsumersAsync
            };

            // CODEREVIEW:: really ought to reuse connectons - best practice seems to
            // suggest one inbound and one outbound connection per process.
            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();
            _defaultConsumer = new RabbitConsumer(_connection, queueName);
            this.QueueName = _defaultConsumer.QueueName;
        }

        public delegate void BytesReceivedEventHandler(BasicDeliverEventArgs args);

        public void StartListening(int numberOfListeners = 1)
        {
            StartConsumer(_defaultConsumer);

            for (int n = 1; n < numberOfListeners; n++)
            {
                var x = new Thread(() =>
                {
                    RabbitConsumer consumer = new RabbitConsumer(_connection, QueueName);
                    StartConsumer(consumer);

                    Console.WriteLine(
                        $"Creating consumer on thread {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                });

                x.Start();
            }
        }

        public void StartListening(ushort prefetch, int numberOfListeners = 1)
        {
            StartSingleMessageConsumer(_defaultConsumer, prefetch);

            for (int n = 1; n < numberOfListeners; n++)
            {
                var x = new Thread(() =>
                {
                    RabbitConsumer consumer = new RabbitConsumer(_connection, QueueName);
                    StartSingleMessageConsumer(consumer, prefetch);

                    Console.WriteLine(
                        $"Creating consumer on thread {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                });

                x.Start();
            }
        }

        public void StopListening()
        {
            foreach (var consumer in _consumers.Values)
            {
                var tag = consumer.ConsumerTag;
                consumer.StopListening();
                consumer.Received -= ConsumersRecievedMessage;
                _consumers.Remove(tag);

                if (_consumers.Count == 0)
                {
                    break;
                }
            }
        }

        protected virtual void OnAcknowlegementSent(BasicDeliverEventArgs args)
        {
            if (args.BasicProperties.ReplyTo != null)
            {
                var channel = _consumers[args.ConsumerTag].Channel;
                IBasicProperties props = channel.CreateBasicProperties();
                props.CorrelationId = args.BasicProperties.CorrelationId;
                string response = "Acknowlegement Sent";
                channel.BasicPublish(string.Empty, args.BasicProperties.ReplyTo, props,
                    Encoding.UTF8.GetBytes(response));
            }
        }

        private void StartConsumer(RabbitConsumer consumer)
        {
            if (string.IsNullOrEmpty(consumer.ConsumerTag))
            {
                consumer.Received += ConsumersRecievedMessage;
                consumer.StartListening();
                _consumers.Add(consumer.ConsumerTag, consumer);
            }
        }

        private void StartSingleMessageConsumer(RabbitConsumer consumer, ushort prefetch)
        {
            if (string.IsNullOrEmpty(consumer.ConsumerTag))
            {
                consumer.Received += ConsumersRecievedMessage;
                consumer.StartListening(prefetch);
                _consumers.Add(consumer.ConsumerTag, consumer);
            }
        }

        private void ConsumersRecievedMessage(object sender, BasicDeliverEventArgs e)
        {
            Trace.TraceInformation("ConsumerRecievedMessage");

            Console.WriteLine(
                $"ConsumerRecievedMessage on thread {System.Threading.Thread.CurrentThread.ManagedThreadId}");

            // CODEREVIEW this line causes unknown issue when it cant find tag this may be due listener being stopped. Need to get to the bottom of it.
            var channel = _consumers[e.ConsumerTag].Channel;

            try
            {
                funcProcessBytesRecieved(e);
                channel.BasicAck(e.DeliveryTag, false);

                try
                {
                    OnAcknowlegementSent(e);
                }
                catch (Exception ex)
                {
                    // DO NOT NACK
                    ex.LogException("ConsumerRecievedMessage - Acknowledging/replying");
                }
            }
            catch (Exception ex)
            {
                ex.LogException("ConsumerRecievedMessage - Processing recieved message- NACK sent");
                channel.BasicNack(e.DeliveryTag, false, true);
            }
        }
    }
}

