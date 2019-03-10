using System;
using System.Diagnostics.Contracts;

namespace Endless.Exceptions.Composition
{
    [ExportAsExceptionLogger(typeof(AggregateException))]
    public class AggregateExceptionLogger : Endless.Exceptions.AggregateExceptionLogger
    {
    }
}