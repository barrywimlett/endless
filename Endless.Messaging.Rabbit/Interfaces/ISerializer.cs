namespace Endless.Messaging.Rabbit.Interfaces
{
    public interface ISerializer
    {
        byte[] Serialize<TMessageBody>(TMessageBody body);

        TMessageBody DeSerialize<TMessageBody>(byte[] jsonBytes);
    }
}