using System.Xml;
using RabbitMQ.Client.Events;

namespace Endless.Messaging.Rabbit
{
    public class XmlDocumentDeliveryEventArgs : TextDeliveryEventArgs
    {
        public XmlDocument XmlDocument { get; set; }

        public XmlDocumentDeliveryEventArgs(ByteRecievedArgs args, string bodyText) : base(args, bodyText)
        {
        }
    }
}