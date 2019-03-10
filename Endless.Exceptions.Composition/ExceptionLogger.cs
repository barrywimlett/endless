using System;
using System.Composition;

namespace Endless.Exceptions.Composition
{
    [Export(typeof(IExceptionLogger))]
    [ExportAsExceptionLogger(typeof(Exception))]
    public class ExceptionLogger : Exceptions.ExceptionLogger
    {
    }
}