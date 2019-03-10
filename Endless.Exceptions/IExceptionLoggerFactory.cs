using System;

namespace Endless.Exceptions
{
    public interface IExceptionLoggerFactory
    {
        void LogException(Exception ex);
    }
}