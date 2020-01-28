using System;
using System.Composition;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;


namespace Endless.Windows
{
    public class ApplicationHelper
    {
        public static class ExceptionHandling
        {

            /// <summary>
            /// Setups the specified application with exception logging for terminal errors.
            /// </summary>
            /// <param name="app">The application.</param>
            public static void Setup(System.Windows.Application app)
            {
                Contract.Requires(app != null);

                app.DispatcherUnhandledException += (sender, args) => Current_DispatcherUnhandledException(app, args);

                TaskScheduler.UnobservedTaskException += (sender, args) => TaskScheduler_UnobservedTaskException(app, args);

                AppDomain.CurrentDomain.UnhandledException += (sender, args) => CurrentDomain_UnhandledException(app, args);
            }

            /// <summary>
            /// Handles the DispatcherUnhandledException event of the Current control.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="System.Windows.Threading.DispatcherUnhandledExceptionEventArgs"/> instance containing the event data.</param>
            private static void Current_DispatcherUnhandledException(Application app,DispatcherUnhandledExceptionEventArgs e)
            {
                e.Handled = true;
                LogException("Dispatcher unhandled exception", e.Exception);
            }

            /// <summary>
            /// Handles the UnhandledException event of the CurrentDomain control.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="UnhandledExceptionEventArgs"/> instance containing the event data.</param>
            private static void CurrentDomain_UnhandledException(Application app, UnhandledExceptionEventArgs e)
            {
                LogException("AppDomain unhandled exception, isTerminating:{0}" + e.IsTerminating,
                    e.ExceptionObject as Exception);
            }

            /// <summary>
            /// Logs the exception.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <param name="e">The e.</param>
            public static void LogException(string context, Exception e)
            {
               
            }

            /// <summary>
            /// Handles the UnobservedTaskException event of the TaskScheduler control.
            /// </summary>
            /// <param name="sender">The source of the event.</param>
            /// <param name="e">The <see cref="UnobservedTaskExceptionEventArgs"/> instance containing the event data.</param>
            private static void TaskScheduler_UnobservedTaskException(Application app, UnobservedTaskExceptionEventArgs e)
            {
                LogException("Unobserved task exception", e.Exception);
            }

            public interface IExceptionLoggerMetaData
            {
                Type ExceptionType { get; }
            }

            [MetadataAttribute]
            [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property,
                AllowMultiple = false)]
            public class ExportAsExceptionLoggerAttribute : Attribute, IExceptionLoggerMetaData
            {
                public ExportAsExceptionLoggerAttribute(Type exceptionType)
                {
                    Contract.Requires(exceptionType.IsSubclassOf(typeof(Exception)),
                        "exception must be subclass of Exception");
                    this.ExceptionType = exceptionType;
                }

                public Type ExceptionType { get; protected set; }
            }

            public interface IExceptionLogger
            {
                void LogException(Exception ex);
            }

            [Export(typeof(IExceptionLogger))]
            [ExportAsExceptionLogger(typeof(Exception))]
            public class ExceptionLogger : IExceptionLogger
            {
                [ImportingConstructor]
                public ExceptionLogger()
                {
                    //this.logger = logger;
                }

                public void LogException(Exception ex)
                {
                    Trace.TraceError(ex.Message);
                    Trace.TraceError(ex.Message);
                }
            }

            /* EntityFramework Specific
             *
            private void HandleDbUpdateException( DbUpdateException dbue)
            {
                if (dbue.InnerException != null)
                {
                    _log.FatalFormat("InnerException");
                    _log.Fatal(dbue.InnerException);
                }
                foreach (var entry in dbue.Entries)
                {
                    _log.FatalFormat("Entity:{0} State:{1}", entry.Entity, entry.State);

                    foreach (var propertyName in entry.CurrentValues.PropertyNames)
                    {
                        object old = null, current = null;
                        if (entry.OriginalValues.PropertyNames.Contains(propertyName))
                        {
                            old = entry.OriginalValues[propertyName];
                        }

                        if (entry.CurrentValues.PropertyNames.Contains(propertyName))
                        {
                            current = entry.CurrentValues[propertyName];
                        }

                        _log.FatalFormat("Property:'{0}' old={1},current={2}", propertyName, old, current);
                    }
                }
            }

            private void HandleEnityValidationException(DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

            }
            */
        }

    }
}