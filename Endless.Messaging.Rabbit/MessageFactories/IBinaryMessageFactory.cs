namespace Endless.Messaging.Rabbit.MessageFactories
{
    public interface IBinaryMessageFactory : IMessageFactory
    {
        RabbitMessage CreateMessage(byte[] data);
        byte[] DecomposeMessage(RabbitMessage message);
    }

   
}