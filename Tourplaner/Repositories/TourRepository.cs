using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
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

        public void Update(Tour value, NpgsqlTransaction transaction = null)
        {
            Assert.NotNull(value, nameof(value));
            Assert.NotNull(value.Route, nameof(value.Route));

            const string statement = @"UPDATE public.""Tour""
                SET ""Name"" = @name, ""Description"" = @description, ""From"" = @from, ""To"" = @to, ""RouteType"" = @routeType
                WHERE ""Tour_ID"" = @tourID;";

            NpgsqlParameter[] parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("name", value.Name),
                PostgreSqlParameterHelper.ValueOrNull("description", value.Description),
                PostgreSqlParameterHelper.ValueOrNull("from", value.Route.From),
                PostgreSqlParameterHelper.ValueOrNull("to", value.Route.To),
                new NpgsqlParameter("routeType", (int)value.Route.RouteType),
                new NpgsqlParameter("tourID", value.ID),
            };

            database.ExecuteNonQuery(statement, transaction, parameters);
        }

        public void Delete(Tour value, NpgsqlTransaction transaction = null)
        {
            Assert.NotNull(value, nameof(value));

            const string statement = @"DELETE FROM public.""Tour""
                WHERE ""Tour_ID"" = @tourID;";

            database.ExecuteNonQuery(statement, transaction, new NpgsqlParameter("tourID", value.ID));
        }

        public IEnumerable<Tour> GetTours(NpgsqlTransaction transaction = null)
        {
            return GetToursWhere(null, transaction, new NpgsqlParameter[0]);
        }

        public IEnumerable<Tour> GetToursWhere(string whereCondition, NpgsqlTransaction transaction = null, params NpgsqlParameter[] parameters)
        {
            const string statement = @"SELECT ""Tour_ID"", ""Name"", 
                ""Description"", ""From"", ""To"", ""RouteType""
                FROM public.""Tour""";

            StringBuilder sql = new StringBuilder(statement);

            if(!string.IsNullOrWhiteSpace(whereCondition))
            {
                sql.Append(" WHERE ");
                sql.Append(whereCondition);
            }

            sql.Append(";");

            return database.Execute(sql.ToString(), transaction, RowSelector, parameters);
        }

        private static Tour RowSelector(NpgsqlDataReader reader)
        {
            return new Tour()
            {
                ID = reader.GetValue<int>("Tour_ID"),
                Name = reader.GetValue<string>("Name"),
                Description = reader.GetValueOrDefault<string>("Description"),
                Route = new RouteInformation()
                {
                    From = reader.GetValueOrDefault<string>("From"),
                    To = reader.GetValueOrDefault<string>("To"),
                    RouteType = (RouteType)reader.GetValue<int>("RouteType")
                }
            };
        }

        private readonly PostgreSqlDatabase database;
    }
}
