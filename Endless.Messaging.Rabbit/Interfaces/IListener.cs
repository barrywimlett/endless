using System;

namespace Endless.Messaging.Rabbit.Interfaces
{
    public interface IListener<TMessageBody>
        where TMessageBody : class
    {
        event Action<IMessageEnvelope<TMessageBody>> MessageReceived;

        void StartListening(int numberOfListeners = 1);

        void StartListening(ushort prefetch, int numberOfListeners = 1);

        void StopListening();
    }
}
