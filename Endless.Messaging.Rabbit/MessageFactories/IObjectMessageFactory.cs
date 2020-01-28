namespace Endless.Messaging.Rabbit.MessageFactories
{
    public interface IObjectMessageFactory : IMessageFactory
    {
        RabbitMessage CreateMessage(object obj);
        object DecomposeMessage(RabbitMessage msg);
    }
}