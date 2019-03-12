using System;
using System.Threading.Tasks;

namespace Endless.Diagnostics.Exceptions
{
    public static class WithExceptionHandling
    {
        public static Task<T> Do<T>(string exceptionContext, Func<Task<T>> operation, Action<Exception> exceptionHandler = null)
        {
            return Do(exceptionContext, operation, DefaultAggregateExceptionHandler, exceptionHandler);
        }

        public static Task Do(string exceptionContext, Func<Task> operation, Action<Exception> exceptionHandler = null)
        {
            return Do(exceptionContext, operation, DefaultAggregateExceptionHandler,exceptionHandler);
        }

        public static Task<T> Do<T>(string exceptionContext, Func<Task<T>> operation, Func<Exception, bool> aggregateExceptionHandler, Action<Exception> exceptionHandler = null)
        {
            try
            {
                return operation.Invoke();
            }
            catch (AggregateException aggEx)
            {
                ExceptionHandling.LogException(exceptionContext, aggEx);
                aggEx.Handle(aggregateExceptionHandler);
                if (exceptionHandler != null)
                {
                    exceptionHandler(aggEx);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(exceptionContext, ex);
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }
            }

            //TODO: is this right ?
            return null;
        }

        public static Task Do(string exceptionContext, Func<Task> operation,Func<Exception,bool> aggregateExceptionHandler,Action<Exception> exceptionHandler=null)
        {
            try
            {
                return operation.Invoke();
            }
            catch (AggregateException aggEx)
            {
                // log first
                ExceptionHandling.LogException( exceptionContext, aggEx);
                // .. handle after
                aggEx.Handle(aggregateExceptionHandler);
                if (exceptionHandler != null)
                {
                    exceptionHandler(aggEx);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandling.LogException(exceptionContext, ex);
                if (exceptionHandler != null)
                {
                    exceptionHandler(ex);
                }
            }
            //TODO: is this right ?
            return null;
        }

        
        private static bool DefaultAggregateExceptionHandler(Exception ex)
        {
            return true;
        }
    }
}