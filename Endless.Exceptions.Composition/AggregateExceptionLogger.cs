using System;

namespace Endless.Diagnostics.Exceptions.Composition
{
    [ExportAsExceptionLogger(typeof(AggregateException))]
    public class AggregateExceptionLogger : Exceptions.AggregateExceptionLogger
    {
    }
}