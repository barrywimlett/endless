namespace Endless.Messaging.Rabbit.MessageFactories
{
    public interface ITextMessageFactory : IMessageFactory
    {
        RabbitMessage CreateMessage(string text);
        string DecomposeMessage(RabbitMessage message);
    }

    
}