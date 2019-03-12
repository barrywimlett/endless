namespace Endless.Messaging.Rabbit.Interfaces
{
    public interface IBasicOutboundRabbitMessage<TBody> : IBasicRabbitMessage<TBody>
        where TBody : class
    {
        string RouteKey { get; }
    }
}