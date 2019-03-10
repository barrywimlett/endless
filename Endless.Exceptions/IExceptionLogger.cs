using System;

namespace Endless.Exceptions
{
    public interface IExceptionLogger
    {
        void LogException(Exception ex);
    }
}