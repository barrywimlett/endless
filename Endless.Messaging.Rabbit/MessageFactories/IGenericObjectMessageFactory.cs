namespace Endless.Messaging.Rabbit.MessageFactories
{
    public interface IGenericObjectMessageFactory<TObject> : IMessageFactory
    {
        RabbitMessage CreateMessage(TObject obj);
        TObject DecomposeMessage(RabbitMessage msg);
    }
}