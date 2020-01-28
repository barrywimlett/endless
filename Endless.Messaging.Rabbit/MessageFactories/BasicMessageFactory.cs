namespace Endless.Messaging.Rabbit.MessageFactories
{
    public class BasicMessageFactory : IBinaryMessageFactory
    {
        public RabbitMessage CreateMessage(byte[] data)
        {
            return new RabbitMessage(data);
        }

        public byte[] DecomposeMessage(RabbitMessage message)
        {
            return message.Data;
        }
    }
}