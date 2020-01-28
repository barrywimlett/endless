using Endless.Messaging.Rabbit.Interfaces;

namespace Endless.Messaging.Rabbit.Generic
{
    public class ListenerFactory<TMessage> : IListenerFactory<TMessage>
        where TMessage : class, new()
    {
        private readonly IRabbitMessagingSettings _messagingSettings;

        public ListenerFactory(IRabbitMessagingSettings messagingSettings)
        {
            _messagingSettings = messagingSettings;
        }

        public IListener<TMessage> Create(string queueName)
        {
            return new RabbitListener<TMessage>(_messagingSettings, queueName);
        }
    }
}