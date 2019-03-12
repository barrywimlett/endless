namespace Endless.Messaging.Rabbit.Interfaces
{
    public interface IListenerFactory<TMessageBody>
        where TMessageBody : class
    {
        IListener<TMessageBody> Create(string queueName);
    }
}
