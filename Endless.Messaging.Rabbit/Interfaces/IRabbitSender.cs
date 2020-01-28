using Endless.Messaging.Rabbit.MessageFactories;

namespace Endless.Messaging.Rabbit.Interfaces
{
    public interface IRabbitSender<TMessageBody> : ISender<TMessageBody>
        where TMessageBody : class
    {
        // void SendBasicMessage(IBasicRabbitMessage<TMessageBody> message);
    }

    public interface IRabbitSender
    {
        void SendMessage(RabbitMessage message);
    }
}