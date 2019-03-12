using System.Diagnostics;
using System.Text;
using RabbitMQ.Client.Events;

namespace Endless.Messaging.Rabbit
{
    public class TextDeliveryEventArgs : ByteRecievedArgs
    {
        public string BodyText { get; set; }

        public TextDeliveryEventArgs(ByteRecievedArgs args, string bodyText) :base(args)
        {
            this.BodyText = bodyText;
            this.Body = args.Body;
            this.BasicProperties = args.BasicProperties;
            this.ConsumerTag = args.ConsumerTag;
            this.DeliveryTag = args.DeliveryTag;
            this.Exchange = args.Exchange;
            this.Redelivered = args.Redelivered;
            this.RoutingKey = RoutingKey;
        }

        public static void decode(ByteRecievedArgs args)
        {
            System.Text.Encoding encoding = Encoding.UTF8;

            var contentEncoding = args.BasicProperties.ContentEncoding;
            if (string.IsNullOrWhiteSpace(contentEncoding))
            {
            }
            else
            {
                switch (contentEncoding)
                {
                    case "utf8":
                        break;
                    case "unicode":
                        encoding = Encoding.Unicode;
                        break;
                    case "utf32":
                        encoding = Encoding.UTF32;
                        break;
                    default:
                        Debug.Assert(false);
                        break;
                }
            }

            string bodyText = encoding.GetString(args.Body);
            TextDeliveryEventArgs textArgs = new TextDeliveryEventArgs(args, bodyText);
        }
    }
}