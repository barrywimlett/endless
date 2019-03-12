using RabbitMQ.Client;

namespace Endless.Messaging.Rabbit.Interfaces
{
    /// <summary>
    /// RabbitMQ talks about messages to include the envelope hence for rabbit classes Message<->Envelope
    /// </summary>
    /// <typeparam name="TBody">Message Type</typeparam>
    public interface IBasicRabbitMessage<TBody> : IMessageEnvelope<TBody>
        where TBody : class
    {
        IBasicProperties BasicProperties { get; }
    }

    /// <summary>
    /// untyped RabbitMQ Message - really just the BasicProperties
    /// </summary>
    public interface IBasicRabbitMessage
    {
        // TODO: maybe this should be byte[] to reflect what form a message arrives from the base rabbit classes
        object MessageBody { get; }

        IBasicProperties BasicProperties { get; }
    }
}