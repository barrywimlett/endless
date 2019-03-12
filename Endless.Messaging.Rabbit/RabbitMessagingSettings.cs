using System.Collections.Generic;
using Endless.Messaging.Rabbit.Interfaces;

namespace Endless.Messaging.Rabbit
{
    public class RabbitMessagingSettings : IRabbitMessagingSettings
    {
        internal const string DefaultName = "Default";

        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMessagingSettings"/> class.
        /// At least force calling code to explicitly state their config values in call
        /// </summary>
        /// <param name="hostname">hostname</param>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <param name="queues">Rabbit MQ queue names</param>
        /// <param name="heartbeatTimeout">heartbeat timeouts</param>
        /// <param name="handshakeTimeout">handshake timeout</param>
        /// <param name="networkRecoveryInterval">network recovery</param>
        public RabbitMessagingSettings(
                string hostname,
                string username,
                string password,
                Dictionary<string, string> queues,
                ushort heartbeatTimeout = 10,
                ushort handshakeTimeout = 2,
                ushort networkRecoveryInterval = 2)
        {
            this.HostName = hostname;
            this.HeartbeatTimeout = heartbeatTimeout; // seconds
            this.HandshakeTimeout = handshakeTimeout; // minutes
            this.NetworkRecoveryInterval = networkRecoveryInterval; // minutes
            this.Username = username;
            this.Password = password;
            this.Queues = queues;
        }

        private RabbitMessagingSettings()
        {
            // the default constructor has been made private **deliberately**
            // as it was been abused by unity configs registering a type using this default-constructor with hard-coded values,
            // instead of an creating and registering instance.
        }

        public string HostName { get; protected set; }

        public ushort HeartbeatTimeout { get; protected set; }

        public ushort HandshakeTimeout { get; protected set; }

        public ushort NetworkRecoveryInterval { get; protected set; }

        public string Username { get; protected set; }

        public string Password { get; protected set; }

        public Dictionary<string, string> Queues { get; protected set; }
    }
}
