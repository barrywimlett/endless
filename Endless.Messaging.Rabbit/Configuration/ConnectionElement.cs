using System.Configuration;

namespace Endless.Messaging.Rabbit.Configuration
{
    public class ConnectionElement : ConfigurationElement
    {
        [System.Configuration.ConfigurationProperty("name", IsKey = true, DefaultValue = RabbitMessagingSettings.DefaultName, IsRequired = false)]
        public string Name => (string)this["name"];

        [ConfigurationProperty("hostname", DefaultValue = "localhost", IsRequired = true)]
        public string HostName => (string)this["hostname"];

        [ConfigurationProperty("heartbeattimeout", DefaultValue = (ushort)10, IsRequired = false)]
        public ushort HeartbeatTimeout => (ushort)this["heartbeattimeout"];

        [ConfigurationProperty("handshaketimeout", DefaultValue = (ushort)2, IsRequired = false)]
        public ushort HandshakeTimeout => (ushort)this["handshaketimeout"];

        [ConfigurationProperty("networkrecoveryinterval", DefaultValue = (ushort)2, IsRequired = false)]
        public ushort NetworkRecoveryInterval => (ushort)this["networkrecoveryinterval"];

        [ConfigurationProperty("username", DefaultValue = "guest", IsRequired = false)]
        public string Username => (string)this["username"];

        [ConfigurationProperty("password", DefaultValue = "guest", IsRequired = false)]
        public string Password => (string)this["password"];

        [ConfigurationProperty("queues", IsDefaultCollection = false)]
        public QueueCollection Queues
        {
            get
            {
                return (QueueCollection)base["queues"];
            }
        }
    }
}
