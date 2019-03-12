using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Endless.Messaging.Rabbit
{
    public abstract class BaseRabbitConsumer
    {
        protected readonly IModel _channel;
        protected string _consumerTag;
        protected int _managedThreadId;

        protected BaseRabbitConsumer(IConnection connection, string listeningQueueName)
        {
            _channel = connection.CreateModel();
            if (string.IsNullOrEmpty(listeningQueueName))
            {
                var declareResult = _channel.QueueDeclare(listeningQueueName, false, true, true, null);
                QueueName = declareResult.QueueName;
            }
            else
            {
                _channel.QueueDeclare(listeningQueueName, true, false, false, null);
                _channel.ExchangeDeclare(listeningQueueName, "direct", true, false, null);
                _channel.QueueBind(listeningQueueName, listeningQueueName, listeningQueueName, null);

                QueueName = listeningQueueName;
            }


            // TODO: need to consider other types of exchange
            _managedThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
        }



        public IModel Channel => _channel;
        public string ConsumerTag => _consumerTag;

        public string QueueName { get; protected set; }

        abstract public void StartListening();

        abstract public void StartListening(ushort prefetch);

        abstract public void StopListening();

        
        
    }

    public class RabbitConsumer : BaseRabbitConsumer
    {
        private EventingBasicConsumer _consumer;

        public RabbitConsumer(IConnection connection, string listeningQueueName) : base(connection, listeningQueueName)
        {
            _consumer = new EventingBasicConsumer(_channel);

        }

        public event EventHandler<BasicDeliverEventArgs> Received
        {
            add
            {
                lock (_consumer)
                {
                    _consumer.Received += value;
                }
            }

            remove
            {
                lock (_consumer)
                {
                    _consumer.Received -= value;
                }
            }
        }

        override public void StartListening()
        {
            if (_consumer == null)
            {
                _consumer = new EventingBasicConsumer(_channel);
            }

            _consumerTag = _channel.BasicConsume(QueueName, false, _consumer);
        }

        override public void StartListening(ushort prefetch)
        {
            if (_consumer == null)
            {
                _consumer = new EventingBasicConsumer(_channel);
            }

            _channel.BasicQos(0, prefetch, false);
            _consumerTag = _channel.BasicConsume(QueueName, false, _consumer);
        }

        override public void StopListening()
        {
            if (_consumer == null)
            {
                _consumer = new EventingBasicConsumer(_channel);
            }

            _channel.BasicCancel(_consumerTag);
            _consumerTag = null;
        }

    }

    public class AsyncRabbitConsumer : BaseRabbitConsumer
    {
        private AsyncEventingBasicConsumer _asyncConsumer;

        public AsyncRabbitConsumer(IConnection connection, string listeningQueueName) : base(connection, listeningQueueName)
        {
            _asyncConsumer = new AsyncEventingBasicConsumer(_channel);

        }

        public event AsyncEventHandler<BasicDeliverEventArgs> ReceivedAsync
        {
            add
            {
                lock (_asyncConsumer)
                {
                    _asyncConsumer.Received += value;
                }
            }

            remove
            {
                lock (_asyncConsumer)
                {
                    _asyncConsumer.Received -= value;
                }
            }
        }

        override public void StartListening()
        {
            if (_asyncConsumer == null)
            {
                _asyncConsumer = new AsyncEventingBasicConsumer(_channel);
            }

            _consumerTag = _channel.BasicConsume(QueueName, false, _asyncConsumer);
        }

        override public void StartListening(ushort prefetch)
        {
            if (_asyncConsumer == null)
            {
                _asyncConsumer = new AsyncEventingBasicConsumer(_channel);
            }

            _channel.BasicQos(0, prefetch, false);
            _consumerTag = _channel.BasicConsume(QueueName, false, _asyncConsumer);
        }

        override public void StopListening()
        {
            if (_asyncConsumer == null)
            {
                _asyncConsumer = new AsyncEventingBasicConsumer(_channel);
            }

            _channel.BasicCancel(_consumerTag);
            _consumerTag = null;
        }

    }

}
