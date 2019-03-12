using System.Collections.Generic;

namespace Endless.Messaging.Rabbit.Interfaces
{
    public interface IRabbitMessagingSettings 
    {
        string HostName { get; }

        ushort HeartbeatTimeout { get; }

        ushort HandshakeTimeout { get; }

        ushort NetworkRecoveryInterval { get; }

        string Username { get; }

        string Password { get; }

        Dictionary<string, string> Queues { get; }
    }
}
