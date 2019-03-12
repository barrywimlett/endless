using Next.Search.Messaging.Rabbit;
using Next.Search.Messaging.Topology.Endpoints;
using Next.Search.Messaging.Topology.Interfaces;
using Unity;

namespace Next.Search.Messaging.Topology
{
    public static class Unity
    {
        public static void RegisterEndpoints(this IUnityContainer container)
        {
            var messagingSettings = RabbitConnectionManager.Singleton.GetDefaultConnectionSetting();

            container.RegisterInstance<IMessagingSettings>(messagingSettings);
            container.RegisterInstance<IRabbitMessagingSettings>(messagingSettings);

            container.RegisterInstance<IRabbitConnectionManager>(RabbitConnectionManager.Singleton);

            container.RegisterType<IStockStatusEndpoint, StockStatusEndpoint>();
            container.RegisterType<IStockStatusSender, StockStatusEndpoint>();
            container.RegisterType<IStockStatusListener, StockStatusEndpoint>();

            container.RegisterType<IPriceStatusEndpoint, PriceStatusEndpoint>();
            container.RegisterType<IPriceStatusSender, PriceStatusEndpoint>();
            container.RegisterType<IPriceStatusListener, PriceStatusEndpoint>();

            container.RegisterType<INewInStatusEndpoint, NewInStatusEndpoint>();
            container.RegisterType<INewInStatusSender, NewInStatusEndpoint>();
            container.RegisterType<INewInStatusListener, NewInStatusEndpoint>();

            container.RegisterType<IStyleItemClassificationEndpoint, StyleItemClassificationEndpoint>();
            container.RegisterType<IStyleItemClassificationSender, StyleItemClassificationEndpoint>();
            container.RegisterType<IStyleItemClassificationListener, StyleItemClassificationEndpoint>();

            container.RegisterType<IPublishedXmlEndpoint, PublishedXmlEndpoint>();
            container.RegisterType<IPublishedXmlSender, PublishedXmlEndpoint>();
            container.RegisterType<IPublishedXmlListener, PublishedXmlEndpoint>();

            container.RegisterType<IJobExecuteEndpoint, JobExecuteEndpoint>();
            container.RegisterType<IJobExecuteSender, JobExecuteEndpoint>();
            container.RegisterType<IJobExecuteListener, JobExecuteEndpoint>();

            container.RegisterType<IJobCompleteEndpoint, JobCompleteEndpoint>();
            container.RegisterType<IJobCompleteSender, JobCompleteEndpoint>();
            container.RegisterType<IJobCompleteListener, JobCompleteEndpoint>();

            container.RegisterType<IRefreshRequestEndpoint, RefreshRequestEndpoint>();
            container.RegisterType<IRefreshRequestListener, RefreshRequestEndpoint>();
            container.RegisterType<IRefreshRequestSender, RefreshRequestEndpoint>();

            container.RegisterType<IRefreshCompleteEndpoint, RefreshCompleteEndpoint>();
            container.RegisterType<IRefreshCompleteListener, RefreshCompleteEndpoint>();
            container.RegisterType<IRefreshCompleteSender, RefreshCompleteEndpoint>();

            container.RegisterType<IJobEndpoint, JobMessageEndpoint>();
            container.RegisterType<IJobSender, JobMessageEndpoint>();
            container.RegisterType<IJobListener, JobMessageEndpoint>();

            container.RegisterType<IFileMessageListener, FileMessageEndpoint>();
            container.RegisterType<IFileMessageSender, FileMessageEndpoint>();
            container.RegisterType<IFileMessageEndpoint, FileMessageEndpoint>();
        }
    }
}