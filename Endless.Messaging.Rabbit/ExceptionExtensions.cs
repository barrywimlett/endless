using System;
using System.Runtime.CompilerServices;

namespace Endless.Messaging.Rabbit.Generic
{
    public static class ExceptionExtensions
    {
        public static void LogException(this Exception ex, [CallerMemberName] string context = null)
        {
            System.Diagnostics.Trace.TraceError("Context:{0}", context);
            System.Diagnostics.Trace.Indent();
            System.Diagnostics.Trace.TraceError("Exception:{0}({1})", ex.Message, ex.GetType().Name);
            System.Diagnostics.Trace.TraceError("StackTrace:{0}", ex.StackTrace);

            if (ex.InnerException != null)
            {
                LogException(ex.InnerException, "InnerException");
            }

            System.Diagnostics.Trace.Unindent();
        }
    }
}