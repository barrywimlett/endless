using System.IO;
using System.Text;
using System.Xml;

namespace Endless.Messaging.Rabbit.MessageFactories
{
    public class TextEncodingMessageFactory : ITextMessageFactory
    {
        public System.Text.Encoding Encoding { get; protected set; }
        public IBinaryMessageFactory BinaryMessageFactory { get; protected set; }
        public TextEncodingMessageFactory(IBinaryMessageFactory binaryMessageFactory, System.Text.Encoding encoding)
        {
            this.Encoding = encoding;
            this.BinaryMessageFactory = binaryMessageFactory;
        }

        public RabbitMessage CreateMessage(string text)
        {
            return CreateMessage(text, BinaryMessageFactory, Encoding);
        }

        public static RabbitMessage CreateMessage(string text, IBinaryMessageFactory binaryMessageFactory, Encoding encoding)
        {
            byte[] data = encoding.GetBytes(text);
            var message = binaryMessageFactory.CreateMessage(data);
            message.BasicProperties.ContentEncoding = encoding.BodyName;

            return message;
        }

        public string DecomposeMessage(RabbitMessage message)
        {
            return  DecomposeMessage(message, BinaryMessageFactory,Encoding);
        }

        public static string DecomposeMessage(RabbitMessage message, IBinaryMessageFactory binaryMessageFactory, Encoding encoding)
        {
            byte[] data = binaryMessageFactory.DecomposeMessage(message);
            string text= encoding.GetString(data);
            
            return text;
        }

    }

    public class JSonSerializer<TObject> : IGenericObjectMessageFactory<TObject>
    {
        public ITextMessageFactory TextMessageFactory { get; protected set; }
        public JSonSerializer(ITextMessageFactory textMessageFactory)
        {
            this.TextMessageFactory = textMessageFactory;
        }
        public RabbitMessage CreateMessage(TObject obj)
        {
            var text=Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            return TextMessageFactory.CreateMessage(text);
        }

        public TObject DecomposeMessage(RabbitMessage msg)
        {
            var text = TextMessageFactory.DecomposeMessage(msg);
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<TObject>(text);
            return obj;
        }
    }

    public class XmlSerializer<TObject> : IGenericObjectMessageFactory<TObject>
    {
        public ITextMessageFactory TextMessageFactory { get; protected set; }
        private System.Xml.Serialization.XmlSerializer ser;
        public XmlSerializer(ITextMessageFactory textMessageFactory)
        {
            this.TextMessageFactory = textMessageFactory;
            ser = new System.Xml.Serialization.XmlSerializer(typeof(TObject));
        }
        public RabbitMessage CreateMessage(TObject obj)
        {
            

            StringBuilder sb=new StringBuilder();
            using (var w = new StringWriter(sb))
            {
                ser.Serialize(w, obj);
                
            }
            
            return TextMessageFactory.CreateMessage(sb.ToString());
        
        }

        public TObject DecomposeMessage(RabbitMessage msg)
        {
            var text = TextMessageFactory.DecomposeMessage(msg);
            var sr = new StringReader(text); 
            TObject obj = (TObject)ser.Deserialize(sr);

            return obj;
        }
    }

}