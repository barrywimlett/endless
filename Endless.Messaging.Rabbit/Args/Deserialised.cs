using RabbitMQ.Client.Events;

namespace Endless.Messaging.Rabbit
{
    public abstract class Deserialised<TData> : TextDeliveryEventArgs
    {
        public TData BodyObject { get; set; }

        public Deserialised(ByteRecievedArgs args, string bodyText) : base(args, bodyText)
        {
        }
    }
}