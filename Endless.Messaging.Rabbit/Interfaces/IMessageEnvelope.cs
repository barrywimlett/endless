namespace Endless.Messaging.Rabbit.Interfaces
{
    public interface IMessageEnvelope<TMessageBody>
    {
        TMessageBody MessageBody { get; set; }
    }
}