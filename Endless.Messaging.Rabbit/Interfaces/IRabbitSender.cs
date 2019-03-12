namespace Endless.Messaging.Rabbit.Interfaces
{
    public interface IRabbitSender<TMessageBody> : ISender<TMessageBody>
        where TMessageBody : class
    {
        // void SendBasicMessage(IBasicRabbitMessage<TMessageBody> message);
    }
}