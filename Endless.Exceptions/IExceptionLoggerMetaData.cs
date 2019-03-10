using System;

namespace Endless.Exceptions
{
    public interface IExceptionLoggerMetaData
    {
        Type ExceptionType { get; }
    }
}