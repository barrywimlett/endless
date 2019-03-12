using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Endless.Diagnostics.Exceptions;

namespace Endless.EntityFramework
{
    //[ExportAsExceptionLogger(typeof(DbEntityValidationException))]
    public class DbEntityValidationExceptionLogger : ExceptionLogger<DbEntityValidationException>
    {
     
        override public void LogException(DbEntityValidationException dbue)
        {
            ExceptionLogger.LogBasicException(dbue);
            // Throw a new DbEntityValidationException with the improved exception message.
            Trace.TraceError("EntityValidationErrors:");

            Trace.Indent();
            foreach (var entityValidationError in dbue.EntityValidationErrors)
            {
                
                var entity = entityValidationError.Entry;
                Trace.TraceError("Entity:{0}", entity);
                Trace.Indent();
                foreach (var validationError in entityValidationError.ValidationErrors)
                {
                    string propertyName = validationError.PropertyName;
                    var oldValue = GetValue(entity, validationError, entity.OriginalValues, propertyName);
                    var currentValue = GetValue(entity, validationError, entity.CurrentValues, propertyName);

                    Trace.TraceError("ErrorMessage:{0}", validationError.ErrorMessage);
                    Trace.TraceError("Property: {0}",propertyName);
                    Trace.TraceError("Original: {0}",oldValue);
                    Trace.TraceError("Current: {0}", currentValue);
                }
                Trace.Unindent();
                
            }
            Trace.Unindent();
        }

        private static object GetValue(DbEntityEntry entity, DbValidationError validationError, DbPropertyValues dbPropertyValues, string propertyName)
        {
            return dbPropertyValues.PropertyNames.Contains(propertyName)
                ? dbPropertyValues[propertyName]
                : string.Empty;
        }
    }

    
}

