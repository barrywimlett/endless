namespace Endless.Messaging.Rabbit.Interfaces
{
    public interface IRabbitConnectionManager
    {
        RabbitConnections GetConnectionDetails(IRabbitMessagingSettings settings);

        IRabbitMessagingSettings GetDefaultConnectionSetting();
    }
}
