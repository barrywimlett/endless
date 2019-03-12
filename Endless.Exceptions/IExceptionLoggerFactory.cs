using System;

namespace Endless.Diagnostics.Exceptions
{
    public interface IExceptionLoggerFactory
    {
        void LogException(Exception ex);
    }
}