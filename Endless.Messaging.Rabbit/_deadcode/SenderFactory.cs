using Endless.Messaging.Rabbit.Interfaces;

namespace Endless.Messaging.Rabbit.Generic
{
    public class SenderFactory<TMessage> : ISenderFactory<TMessage>
        where TMessage : class, new()
    {
        protected readonly IRabbitMessagingSettings _messagingSettings;

        public SenderFactory(IRabbitMessagingSettings messagingSettings)
        {
            _messagingSettings = messagingSettings;
        }

        public ISender<TMessage> Create(string queueName)
        {
            return new RabbitSender<TMessage>(_messagingSettings, queueName);
        }
    }
}