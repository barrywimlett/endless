using System;
using System.Diagnostics;

namespace Endless.Diagnostics.Exceptions
{
    
    public abstract class ExceptionLogger<TException> :  IExceptionLogger where TException:Exception 
    {
        void IExceptionLogger.LogException(Exception ex)
        {
            TException dbue = ex as TException;
            if (dbue != null)
            {
                this.LogException(dbue);
            }
        }

        public abstract void LogException(TException ex);

    }

    public class ExceptionLogger : ExceptionLogger<Exception>
    {
        public static void LogBasicException(Exception ex)
        {
            Trace.TraceError(@"Type:{0}", ex.GetType());
            Trace.TraceError(@"Message:{0}", ex.Message);
            Trace.TraceError(@"Source:{0}", ex.Source);
            Trace.TraceError(@"Stacktrace:{0}", ex.StackTrace);
        }
        override public void LogException(Exception ex)
        {
            ExceptionLogger.LogBasicException( ex);
        }
    }

}