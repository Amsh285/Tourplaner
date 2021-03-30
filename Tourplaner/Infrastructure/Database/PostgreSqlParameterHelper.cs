using Npgsql;
using System;

namespace Tourplaner.Infrastructure.Database
{
    public static class PostgreSqlParameterHelper
    {
        public static NpgsqlParameter ValueOrNull(string columnName, object value)
        {
            if (value is null)
                return new NpgsqlParameter(columnName, DBNull.Value);
            else
                return new NpgsqlParameter(columnName, value);
        }
    }
}
