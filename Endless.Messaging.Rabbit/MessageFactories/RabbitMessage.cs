using RabbitMQ.Client;
using RabbitMQ.Client.Framing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endless.Messaging.Rabbit.MessageFactories
{
    public class RabbitMessage
    {
        public IBasicProperties BasicProperties { get; protected set; }

        public byte[] Data { get; protected set; }

        public RabbitMessage(byte[] data)
        {
            BasicProperties = new BasicProperties();
            this.Data = data;
        }

        public RabbitMessage(IBasicProperties properties,byte[] data)
        {
            BasicProperties = properties;
            this.Data = data;
        }

    }
}
