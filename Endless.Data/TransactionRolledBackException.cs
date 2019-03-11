using System;
using System.Runtime.Serialization;

namespace Endless.Data
{
    public class TransactionRolledBackException : Exception
    {
        private const string message = @"Transaction Rolled-back";

        public TransactionRolledBackException() : base(message)
        {
            
        }

        protected TransactionRolledBackException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        
        public TransactionRolledBackException(Exception innerException) : base(message, innerException)
        {
        }
    }
}