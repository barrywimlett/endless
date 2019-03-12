namespace Endless.Messaging.Rabbit.Interfaces
{
    public interface IRabbitListener
    {
        void StartListening(int numberOfListeners = 1);

        void StartListening(ushort prefetch, int numberOfListeners = 1);

        void StopListening();
    }
}