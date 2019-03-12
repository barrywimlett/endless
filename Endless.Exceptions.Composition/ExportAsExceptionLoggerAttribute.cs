using System;
using System.Composition;
using System.Diagnostics.Contracts;

namespace Endless.Diagnostics.Exceptions.Composition
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property,
        AllowMultiple = false)]
    public class ExportAsExceptionLoggerAttribute : ExportAttribute, IExceptionLoggerMetaData
    {
        public ExportAsExceptionLoggerAttribute(Type exceptionType)
        {
            Contract.Requires(exceptionType.IsSubclassOf(typeof(Exception)), "exception must be subclass of Exception");
            this.ExceptionType = exceptionType;
        }

        public Type ExceptionType { get; protected set; }
    }
}
