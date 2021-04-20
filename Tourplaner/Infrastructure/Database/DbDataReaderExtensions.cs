using System;
using System.Data.Common;

namespace Tourplaner.Infrastructure.Database
{
    public static class DbDataReaderExtensions
    {
        public static TValue GetValue<TValue>(this DbDataReader reader, string columnName)
        {
            return (TValue)reader.GetValue(columnName);
        }

        public static TValue GetValueOrDefault<TValue>(this DbDataReader reader, string columnName)
        {
            object value = reader.GetValue(columnName);

            if (value == DBNull.Value)
                return default;

            return (TValue)value;
        }

        public static object GetValue(this DbDataReader reader, string columnName)
        {
            Assert.NotNull(reader, nameof(reader));
            Assert.NotNull(columnName, nameof(columnName));

            int index = reader.GetOrdinal(columnName);
            return reader.GetValue(index);
        }
    }
}
