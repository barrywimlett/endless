using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Endless.Exceptions.Composition
{
    [Shared]
    public class ExceptionLoggerFactory
    {
        [ImportMany]
        private IEnumerable<ExportFactory<IExceptionLogger, IExceptionLoggerMetaData>> factories { get; set; }


        private IEnumerable<IExceptionLogger> GetLoggers(Exception exception)
        {
            IList<IExceptionLogger> loggers = new List<IExceptionLogger>();
            var exceptionType = exception.GetType();
            var suitableFactories = this.factories.Where(f => f.Metadata.ExceptionType.IsInstanceOfType(exception));
            int minDistance = suitableFactories.Min(f => DistanceFromBaseClass(exceptionType, f.Metadata.ExceptionType));
            var bestFactories =
                suitableFactories.Where(
                    f => DistanceFromBaseClass(exceptionType, f.Metadata.ExceptionType) == minDistance);

            return bestFactories.Select(f => f.CreateExport().Value);

        }

        public void LogException(Exception ex)
        {
            var loggers = GetLoggers(ex);
            foreach (var logger in loggers)
            {
                logger.LogException(ex);
            }
        }

        private int DistanceFromBaseClass(Type childType, Type parentType)
        {
            Contract.Assume(childType.IsSubclassOf(parentType));
            int distance = 0;
            while (childType != parentType)
            {
                childType = childType.BaseType;
                distance++;
            }
            return distance;

        }
    }
}