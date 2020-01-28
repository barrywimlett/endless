using System;
using Endless.Messaging.Rabbit.Interfaces;
using RabbitMQ.Client;

namespace Endless.Messaging.Rabbit
{
    /// <summary>
    /// Maintains the ConnectionFactory and Connections for a connection setting
    /// </summary>
    public class RabbitConnections
    {
        private IConnection _outboundConnection;
        private IConnection _inboundConnection;

        public RabbitConnections(IRabbitMessagingSettings messageSettings)
        {
            Settings = messageSettings;

            Factory = new ConnectionFactory
            {
                HostName = messageSettings.HostName,
                AutomaticRecoveryEnabled = true,
                RequestedHeartbeat = messageSettings.HeartbeatTimeout,
                HandshakeContinuationTimeout = new TimeSpan(0, messageSettings.HandshakeTimeout, 0),
                NetworkRecoveryInterval = new TimeSpan(0, messageSettings.NetworkRecoveryInterval, 0),
                UserName = messageSettings.Username,
                Password = messageSettings.Password
            };
        }

        public IRabbitMessagingSettings Settings { get; protected set; }

        public bool HasInboundConnection => _inboundConnection != null;

        public bool HasOuboundConnection => _outboundConnection != null;

        public RabbitMQ.Client.IConnection InboundConnection
        {
            get
            {
                if (!HasInboundConnection)
                {
                    _inboundConnection = Factory.CreateConnection();
                }

                return _inboundConnection;
            }

            protected set => _inboundConnection = value;
        }

        public RabbitMQ.Client.IConnection OutboundConnection
        {
            get
            {
                if (!HasOuboundConnection)
                {
                    _outboundConnection = Factory.CreateConnection();
                }

                return _outboundConnection;
            }

            protected set
            {
                _outboundConnection = value;
            }
        }

        protected ConnectionFactory Factory { get; set; }
    }
}