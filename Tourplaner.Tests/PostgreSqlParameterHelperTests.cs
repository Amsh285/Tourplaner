using Npgsql;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Tourplaner.Infrastructure.Database;

namespace Tourplaner.Tests
{
    public sealed class PostgreSqlParameterHelperTests
    {
        [Test]
        public void ValueOrNullNotNullUsesValue()
        {
            const int value = 123;

            NpgsqlParameter parameter = PostgreSqlParameterHelper.ValueOrNull("foo", value);
            Assert.IsInstanceOf<int>(parameter.Value);
            Assert.AreEqual(value, parameter.Value);
        }

        [Test]
        public void ValueOrNullNullUsesDBNull()
        {
            NpgsqlParameter parameter = PostgreSqlParameterHelper.ValueOrNull("foo", null);
            Assert.IsInstanceOf<DBNull>(parameter.Value);
            Assert.AreEqual(DBNull.Value, parameter.Value);
        }

        [Test]
        public void ValueOrNullUsesColumnName()
        {
            const string parameterName = "foo";
            NpgsqlParameter parameter = PostgreSqlParameterHelper.ValueOrNull(parameterName, 123);
            NpgsqlParameter nullParameter = PostgreSqlParameterHelper.ValueOrNull(parameterName, null);

            Assert.AreEqual(parameterName, parameter.ParameterName);
            Assert.AreEqual(parameterName, nullParameter.ParameterName);
        }
    }
}
