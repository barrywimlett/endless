using Endless.Messaging.Rabbit.Interfaces;
using RabbitMQ.Client;

namespace Endless.Messaging.Rabbit.Generic
{
    public class RabbitConnection
    {
        internal readonly IConnection _connection;
        protected IModel _channel;
        private bool _dispatchConsumersAsync;
        protected RabbitConnection(IRabbitMessagingSettings messageSettings, bool dispatchConsumersAsync)

        {

            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = messageSettings.HostName,
                RequestedHeartbeat = messageSettings.HeartbeatTimeout,
                AutomaticRecoveryEnabled = true,
                HandshakeContinuationTimeout = new System.TimeSpan(0, messageSettings.HandshakeTimeout, 0),
                NetworkRecoveryInterval = new System.TimeSpan(0, messageSettings.NetworkRecoveryInterval, 0),
                UserName = messageSettings.Username,
                Password = messageSettings.Password,

                DispatchConsumersAsync = _dispatchConsumersAsync
            };

            // CODEREVIEW:: really ought to reuse connectons - best practice seems to
            // suggest one inbound and one outbound connection per process.
            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();
        }
    }
}