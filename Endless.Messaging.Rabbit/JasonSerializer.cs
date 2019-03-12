using System.Text;
using Endless.Messaging.Rabbit.Interfaces;
using Newtonsoft.Json;

namespace Endless.Messaging.Rabbit
{
    public class JasonSerializer : ISerializer
    {
        public byte[] Serialize<TMessageBody>(TMessageBody body)
        {
            var messageBody = JsonConvert.SerializeObject(body);
            var jsonBytes = Encoding.UTF8.GetBytes(messageBody);
            return jsonBytes;
        }

        public TMessageBody DeSerialize<TMessageBody>(byte[] jsonBytes)
        {
            var jsonString = Encoding.UTF8.GetString(jsonBytes);
            var messageBody = JsonConvert.DeserializeObject<TMessageBody>(jsonString);
            return messageBody;
        }
    }
}