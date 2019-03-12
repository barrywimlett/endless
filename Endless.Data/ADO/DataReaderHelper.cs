using System;
using System.Data;

namespace Endless.Data.ADO
{
    public static class DataReaderHelper
    {

        public static TValue GetValue<TValue>(this IDataReader reader, string columnName,TValue defaultForNull = default(TValue))
        {
            TValue value = defaultForNull;
            int ordinal = reader.GetOrdinal(columnName);
            if (ordinal == -1)
            {
                throw new ArgumentOutOfRangeException($"DataReader does not have column {columnName}");
            }
            else if (reader.IsDBNull(ordinal))
            {
                // null found
            }
            else
            {
                value = (TValue) reader.GetValue(ordinal);
            }

            return value;
        }
        
    }
}