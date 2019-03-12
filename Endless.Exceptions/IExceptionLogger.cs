using System;

namespace Endless.Diagnostics.Exceptions
{
    public interface IExceptionLogger
    {
        void LogException(Exception ex);
    }
}