
using Unity;

namespace Next.Search.Messaging.Rabbit.Unity
{
    static public class UnitRegistrationHelper 
    {

        static public void RegisterRabbitDefaultConnectionSettings(this IUnityContainer container)
        {
            var messagingSettings = RabbitConnectionManager.Singleton.GetDefaultConnectionSetting();
            container.RegisterInstance<IRabbitMessagingSettings>(messagingSettings);

        }
    }
}
