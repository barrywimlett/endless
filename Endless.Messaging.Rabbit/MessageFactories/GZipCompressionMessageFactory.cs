using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;

namespace Endless.Messaging.Rabbit.MessageFactories
{

    public interface IStreamMessageFactory : IMessageFactory
    {
        RabbitMessage CreateMessage(StreamReader stream);

        byte[] DecomposeMessage(StreamWriter writer);
    }

    public class GZipCompressionMessageFactory : IBinaryMessageFactory
    {
        public IBinaryMessageFactory BinaryMessageFactory { get; protected set; }
        public CompressionLevel CompressionLevel { get; protected set; }
        public GZipCompressionMessageFactory(IBinaryMessageFactory binaryMessageFactory,CompressionLevel compressionLevel= CompressionLevel.Optimal)
        {
            this.CompressionLevel = compressionLevel;            
            this.BinaryMessageFactory = binaryMessageFactory;
        }

        public RabbitMessage CreateMessage(byte[] data)
        {
            return CreateMessage(this.BinaryMessageFactory,data, this.CompressionLevel);
        }

        public byte[] DecomposeMessage( RabbitMessage msg)
        {
            return DecomposeMessage(this.BinaryMessageFactory, msg);
        }

        public static RabbitMessage CreateMessage(IBinaryMessageFactory binaryMessageFactory, byte[] data,CompressionLevel compressionLevel)
        {
            try
            {
                using (var ms = new MemoryStream())
                {
                    using (var gzips = new GZipStream(ms, compressionLevel))
                    {
                        gzips.Write(data, 0, data.Length);
                        gzips.Flush();
                    }
                    int compressedDataLength = (int)ms.Length;
                    byte[] compressedData = new byte[compressedDataLength];
                    ms.Read(compressedData, 0, compressedDataLength);
                    var msg = binaryMessageFactory.CreateMessage(compressedData);
                    return msg;
                }
            } catch(Exception ex )
            {
                throw;
            }
        }

      
        public static byte[] DecomposeMessage(IBinaryMessageFactory binaryMessageFactory, RabbitMessage msg)
        {
            try
            {
                byte[] compressedData = binaryMessageFactory.DecomposeMessage(msg);
                using (var ms = new MemoryStream())
                {
                    using (var gzips = new GZipStream(ms, CompressionMode.Decompress))
                    {
                        
                        gzips.Write(compressedData, 0, compressedData.Length);
                        gzips.Flush();
                    }
                    int uncompressedDataLength = (int)ms.Length;
                    byte[] uncompressedData = new byte[uncompressedDataLength];
                    ms.Read(compressedData, 0, uncompressedDataLength);
                    return uncompressedData;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}