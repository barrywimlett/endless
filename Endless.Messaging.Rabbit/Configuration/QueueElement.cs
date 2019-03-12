using System.Configuration;

namespace Endless.Messaging.Rabbit.Configuration
{
    public class QueueElement : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true, IsKey = true, DefaultValue = "")]
        public string Key => (string)this["key"];

        [ConfigurationProperty("value", IsRequired = true, IsKey = true, DefaultValue = "")]
        public string Value => (string)this["value"];
    }
}