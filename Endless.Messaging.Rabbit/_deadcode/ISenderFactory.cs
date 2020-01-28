namespace Endless.Messaging.Rabbit.Interfaces
{
    public interface ISenderFactory<TMessageBody>
        where TMessageBody : class
    {
        ISender<TMessageBody> Create(string queueName);
    }
}
