using System;
using System.Diagnostics.Contracts;

namespace Endless.Diagnostics.Exceptions
{
    
    public class AggregateExceptionLogger : ExceptionLogger<AggregateException>
    {
        
        public override void LogException(AggregateException aggex)
        {
            
            Contract.Assume(aggex != null, "exception must be subclass of AggregateException");

            System.Diagnostics.Debug.WriteLine("InnerExceptions");
            System.Diagnostics.Debug.Indent();
            foreach (var innerEx in aggex.InnerExceptions)
            {
                ExceptionHandling.LogException("InnerException",innerEx);
            }
            System.Diagnostics.Debug.Unindent();


        }
    }
}