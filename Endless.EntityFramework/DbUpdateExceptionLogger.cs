using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using Endless.Diagnostics.Exceptions;

namespace Endless.EntityFramework
{
    //[ExportAsExceptionLogger(typeof (DbUpdateException))]
    public class DbUpdateExceptionLogger : ExceptionLogger<DbUpdateException>
    {
        override public void LogException(DbUpdateException dbue)
        {
            ExceptionLogger.LogBasicException(dbue);

            foreach (var entry in dbue.Entries)
            {
                Trace.TraceError("Entity:{0} State:{1}", entry.Entity, entry.State);

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

                    Trace.TraceError("Property:'{0}' old={1},current={2}", propertyName, old, current);
                }
            }
        }
        
    }
}