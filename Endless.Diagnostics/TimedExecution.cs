using System;
using System.Diagnostics;

namespace Endless.Diagnostics
{
    public static class TimedExecution
    {
        public static void Do(string description, Action action,TimeSpan? warnAfter=null,TimeSpan? errorAfter=null)
        {
            DateTime start = DateTime.UtcNow;
            Trace.TraceInformation($"Started {description}");
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Exception thrown during timed section");
                Trace.TraceError(ex.Message);
            }
            finally
            {
                DateTime end = DateTime.UtcNow;
                ReportTiming(description, warnAfter, errorAfter, end, start);
            }
            
        }

        public static TResult Do<TResult>(string description, Func<TResult> function, TimeSpan? warnAfter = null, TimeSpan? errorAfter = null)
        {
            DateTime start = DateTime.UtcNow;
            Trace.TraceInformation($"Started {description}");
            try
            {
                return function();
            }
            catch (Exception ex)
            {
                Trace.TraceError($"Exception thrown during timed section");
                Trace.TraceError(ex.Message);
                throw;
            }
            finally
            {
                DateTime end = DateTime.UtcNow;
                ReportTiming(description, warnAfter, errorAfter, end, start);
            }

        }

        static private void ReportTiming(string description, TimeSpan? warnAfter, TimeSpan? errorAfter, DateTime end,
            DateTime start)
        {
            var duration = end - start;
            if (errorAfter.HasValue && duration > -errorAfter)
            {
                Trace.TraceError(($"Finished {description} took {duration}"));
            }
            else if (warnAfter.HasValue && duration > -warnAfter)
            {
                Trace.TraceWarning($"Finished {description} took {duration}");
            }
            else
            {
                Trace.TraceInformation($"Finished {description} took {duration}");
            }
        }
    }
}