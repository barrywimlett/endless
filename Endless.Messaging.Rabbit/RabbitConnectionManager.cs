using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Linq;
using Endless.Messaging.Rabbit.Configuration;
using Endless.Messaging.Rabbit.Interfaces;

namespace Endless.Messaging.Rabbit
{
    /// <summary>
    /// Manages connection settings and created connections
    /// </summary>
    public class RabbitConnectionManager : IRabbitConnectionManager
    {
        public static readonly RabbitConnectionManager Singleton = new RabbitConnectionManager();

        private static readonly Lazy<RabbitConfigurationSection> LazyConfigSection =
            new Lazy<RabbitConfigurationSection>(GetConfigSection);

        private readonly Lazy<Dictionary<string, IRabbitMessagingSettings>> lazyConnectionSettings =
            new Lazy<Dictionary<string, IRabbitMessagingSettings>>(GetConnectionSettingsFromConfig);

        private Dictionary<int, RabbitConnections> connectionFactories = new Dictionary<int, RabbitConnections>();

        public IReadOnlyDictionary<string, IRabbitMessagingSettings> ConnectionSettings =>
            lazyConnectionSettings.Value;

        /// <summary>
        /// Gets the DefaultConnectionName in config
        /// </summary>
        private string DefaultConnectionName => ConfigSection.DefaultConnectionName;

        protected static RabbitConfigurationSection ConfigSection => LazyConfigSection.Value;

        /// <summary>
        /// Get named ConnectionSettings from config
        /// </summary>
        /// <param name="connectionName">Connection name</param>
        /// <param name="thowExceptionOnSettingsNotFound">throw exception</param>
        /// <returns>rabbit message settings</returns>
        private IRabbitMessagingSettings GetConnectionSettings(string connectionName, bool thowExceptionOnSettingsNotFound = true)
        {
            IRabbitMessagingSettings messageSettings = null;
            if (!ConnectionSettings.ContainsKey(connectionName))
            {
                if (thowExceptionOnSettingsNotFound)
                {
                    throw new ApplicationException($"Failed to find connection settings with name'{connectionName}'");
                }
            }
            else
            {
                messageSettings = ConnectionSettings[connectionName];
            }

            return messageSettings;
        }

        /// <summary>
        /// Get the active ConnectionDetails for a ConnectionSettings
        /// </summary>
        /// <param name="settings">the settings</param>
        /// <returns>Rabbit Connection</returns>
        public RabbitConnections GetConnectionDetails(IRabbitMessagingSettings settings)
        {
            int hashcode = GetHashCode(settings);
            RabbitConnections factory = null;
            lock (connectionFactories)
            {
                if (connectionFactories.ContainsKey(hashcode))
                {
                    factory = connectionFactories[hashcode];
                }
                else
                {
                    factory = new RabbitConnections(settings);
                    connectionFactories.Add(hashcode, factory);
                }
            }

            return factory;
        }

        /// <summary>
        /// Get the ConnectionSettings for the "default" connection in config
        /// </summary>
        /// <returns>rabbit message settings</returns>
        public IRabbitMessagingSettings GetDefaultConnectionSetting()
        {
            IRabbitMessagingSettings defaultConnectionSettings = null;

            var defaultName = DefaultConnectionName;

            if (string.IsNullOrEmpty(defaultName))
            {
                defaultConnectionSettings = MapConnectionSettings(ConfigSection.ConnectionSettings.FirstOrDefault());
            }
            else
            {
                defaultConnectionSettings = GetConnectionSettings(defaultName);
                if (defaultConnectionSettings == null)
                {
                    defaultConnectionSettings = MapConnectionSettings(ConfigSection.ConnectionSettings.FirstOrDefault());
                }
            }

            Contract.Assume(defaultConnectionSettings != null);

            return defaultConnectionSettings;
        }

        /// <summary>
        /// Calculate the unique hash for a connectionSettings
        /// </summary>
        /// <param name="settings">the settings</param>
        /// <returns>number</returns>
        private int GetHashCode(IRabbitMessagingSettings settings)
        {
            return HashCodeExtension.GetHashCode(
                53777,
                16777619,
                new object[] { settings.HostName, settings.Username, settings.Password, settings.HeartbeatTimeout, settings.NetworkRecoveryInterval, settings.HandshakeTimeout });
        }

        private static RabbitConfigurationSection GetConfigSection()
        {
            var configSection = RabbitConfigurationSection.GetSection();
            return configSection;
        }

        private static Dictionary<string, IRabbitMessagingSettings> GetConnectionSettingsFromConfig()
        {
            var result = new Dictionary<string, IRabbitMessagingSettings>();

            foreach (var conection in ConfigSection.ConnectionSettings)
            {
                var config = conection as ConnectionElement;

                if (config == null)
                {
                    throw new ConfigurationErrorsException("Unable to find rabbit configuration section");
                }

                var setting = MapConnectionSettings(config);

                result.Add(config.Name, setting);
            }

            return result;
        }

        private static IRabbitMessagingSettings MapConnectionSettings(ConnectionElement config)
        {
            var queues = config.Queues.ToDictionary(q => q.Key, q => q.Value);

            var setting = new RabbitMessagingSettings(
                hostname: config.HostName,
                username: config.Username,
                password: config.Password,
                queues: new Dictionary<string, string>(queues),
                heartbeatTimeout: config.HeartbeatTimeout,
                handshakeTimeout: config.HandshakeTimeout,
                networkRecoveryInterval: config.NetworkRecoveryInterval);

            return setting;
        }
    }
}