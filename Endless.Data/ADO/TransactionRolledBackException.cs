using System;
using System.Runtime.Serialization;

namespace Endless.Data.ADO
{

    public class AdoHelperException : Exception
    {
        

        public AdoHelperException(string message) : base(message)
        {

        }

        protected AdoHelperException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }


        public AdoHelperException(string message,Exception innerException) : base(message, innerException)
        {
        }
    }

}