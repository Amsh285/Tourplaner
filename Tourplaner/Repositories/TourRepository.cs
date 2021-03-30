using Npgsql;
using System;
using Tourplaner.Infrastructure;
using Tourplaner.Infrastructure.Database;
using Tourplaner.Models;

namespace Tourplaner.Repositories
{
    public sealed class TourRepository
    {
        public TourRepository(PostgreSqlDatabase database)
        {
            Assert.NotNull(database, nameof(database));

            this.database = database;
        }

        public int Insert(Tour value, NpgsqlTransaction transaction = null)
        {
            Assert.NotNull(value, nameof(value));
            Assert.NotNull(value.Route, nameof(value.Route));

            const string statement = @"INSERT INTO public.""Tour""(
                ""Name"", ""Description"", ""From"", ""To"", ""RouteType"")
	            VALUES(@name, @description, @from, @to, @routeType)
                RETURNING ""Tour_ID""; ";

            NpgsqlParameter[] parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("name", value.Name),
                PostgreSqlParameterHelper.ValueOrNull("description", value.Description),
                PostgreSqlParameterHelper.ValueOrNull("From", value.Route.From),
                PostgreSqlParameterHelper.ValueOrNull("to", value.Route.To),
                new NpgsqlParameter("routeType", (int)value.Route.RouteType),
            };

            return database.ExecuteScalar<int>(statement, transaction, parameters);
        }

        private readonly PostgreSqlDatabase database;
    }
}
