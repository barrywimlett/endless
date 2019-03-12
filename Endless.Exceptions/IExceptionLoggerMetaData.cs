using System;

namespace Endless.Diagnostics.Exceptions
{
    public interface IExceptionLoggerMetaData
    {
        Type ExceptionType { get; }
    }
}