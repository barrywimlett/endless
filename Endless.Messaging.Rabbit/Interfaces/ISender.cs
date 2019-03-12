namespace Endless.Messaging.Rabbit.Interfaces
{
    public interface ISender<TMessageBody>
        where TMessageBody : class
    {
        IMessageEnvelope<TMessageBody> GetEnvelope(TMessageBody body);

        void Send(IMessageEnvelope<TMessageBody> message);

        void Send(TMessageBody message);
    }
}
