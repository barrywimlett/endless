using System.Configuration;

namespace Endless.Messaging.Rabbit.Configuration
{
    public class RabbitConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("default", IsRequired = false, DefaultValue = RabbitMessagingSettings.DefaultName)]
        public string DefaultConnectionName
        {
            get
            {
                return (string)this["default"];
            }

            set
            {
                this["default"] = value;
            }
        }

        [ConfigurationProperty("", IsDefaultCollection = true)]
        internal ConnectionCollection ConnectionSettings
        {
            get
            {
                ConnectionCollection connectionCollection = (ConnectionCollection)base[""];
                return connectionCollection;
            }
        }

        public static RabbitConfigurationSection GetSection(string sectionName = "rabbitConfigurationSection")
        {
            var configuration = ConfigurationManager.GetSection(sectionName) as RabbitConfigurationSection;

            if (configuration == null)
            {
                throw new ConfigurationErrorsException("Unable to find rabbitConfigurationSection in config file");
            }

            return configuration;
        }
    }
}