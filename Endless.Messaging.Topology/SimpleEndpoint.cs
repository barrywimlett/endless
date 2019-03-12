using System;
using Next.Search.Messaging.Generic;
using Next.Search.Messaging.Rabbit;
using Next.Search.Messaging.Rabbit.Generic;

namespace Next.Search.Messaging.Topology
{
    public abstract class SimpleEndpoint<TMessageBody> : IEndpoint<TMessageBody>
        where TMessageBody : class, new()
    {
        private IListener<TMessageBody> _listener;
        private ISender<TMessageBody> _sender;
        private string _queueName;

        public SimpleEndpoint(IRabbitMessagingSettings settings)
        {
            settings.Queues.TryGetValue(this.GetType().Name.ToLower(), out _queueName);
            _listener = new RabbitListener<TMessageBody>(settings, QueueName);
            _sender = new RabbitSender<TMessageBody>(settings, QueueName);
        }

        public event Action<IMessageEnvelope<TMessageBody>> MessageReceived
        {
            add { _listener.MessageReceived += value; }
            remove { _listener.MessageReceived -= value; }
        }

        public string QueueName
        {
            get { return _queueName; }
        }

        public IMessageEnvelope<TMessageBody> GetEnvelope(TMessageBody body)
        {
            return _sender.GetEnvelope(body);
        }

        public void Send(IMessageEnvelope<TMessageBody> message)
        {
            _sender.Send(message);
        }

        public void Send(TMessageBody message)
        {
            _sender.Send(message);
        }

        public void StartListening(int numberOfListeners = 1)
        {
            _listener.StartListening(numberOfListeners);
        }

        public void StartListening(ushort prefetch, int numberOfListeners = 1)
        {
            _listener.StartListening(prefetch, numberOfListeners);
        }

        public void StopListening()
        {
            _listener.StopListening();
        }
    }
}