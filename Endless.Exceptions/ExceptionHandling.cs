using System;

namespace Endless.Exceptions
{
    public static class ExceptionHandling
    {

        //private static readonly Lazy<IExceptionLoggerFactory> factory = new Lazy<IExceptionLoggerFactory>( () => DependencyResolver.Instance.Export<IExceptionLoggerFactory>());

        /// <summary>
        /// Logs the exception.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="e">The e.</param>
        public static void LogException(string context, Exception exception)
        {
            ExceptionHandling.LogException(context,exception);
        }

    }
}