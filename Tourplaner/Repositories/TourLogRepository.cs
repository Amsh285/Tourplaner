using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using Tourplaner.Infrastructure;
using Tourplaner.Infrastructure.Database;
using Tourplaner.Models;

namespace Tourplaner.Repositories
{
    public sealed class TourLogRepository
    {
        public TourLogRepository(PostgreSqlDatabase database)
        {
            Assert.NotNull(database, nameof(database));

            this.database = database;
        }

        public int Insert(TourLog value, int tourID, NpgsqlTransaction transaction = null)
        {
            Assert.NotNull(value, nameof(value));

            const string statement = @"INSERT INTO public.""TourLog""(
                ""Tour_ID"", ""TourDate"", ""Distance"", ""AvgSpeed"", ""Breaks"", ""Brawls"", ""Abductions"",
                ""HobgoblinSightings"", ""UFOSightings"", ""TotalTime"", ""rating"")
                VALUES(@tourID, @tourDate, @distance, @avgSpeed, @breaks, @brawls, @abductions, 
                @hobgoblinSightings, @ufoSightings, @totalTime, @rating)
                RETURNING ""TourLog_ID"";";

            NpgsqlParameter[] parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("tourID", tourID),
                new NpgsqlParameter("tourDate", value.TourDate),
                new NpgsqlParameter("distance", value.Distance),
                new NpgsqlParameter("avgSpeed", value.AvgSpeed),
                new NpgsqlParameter("breaks", value.Breaks),
                new NpgsqlParameter("brawls", value.Brawls),
                new NpgsqlParameter("abductions", value.Abductions),
                new NpgsqlParameter("hobgoblinSightings", value.HobgoblinSightings),
                new NpgsqlParameter("ufoSightings", value.UFOSightings),
                new NpgsqlParameter("totalTime", value.TotalTime),
                new NpgsqlParameter("rating", value.Rating),
            };

            return database.ExecuteScalar<int>(statement, transaction, parameters);
        }

        public void Update(TourLog value, NpgsqlTransaction transaction = null)
        {
            Assert.NotNull(value, nameof(value));

            const string statement = @"UPDATE public.""TourLog""
                SET ""TourDate"" = @tourDate, ""Distance"" = @distance, ""AvgSpeed"" = @avgSpeed,
                ""Breaks"" = @breaks, ""Brawls"" = @brawls, ""Abductions"" = @abductions,
                ""HobgoblinSightings"" = @hobgoblinSightings, ""UFOSightings"" = @ufoSightings,
                ""TotalTime"" = @totalTime, rating = @rating
                WHERE ""TourLog_ID"" = @tourlogID;";

            NpgsqlParameter[] parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("tourDate", value.TourDate),
                new NpgsqlParameter("distance", value.Distance),
                new NpgsqlParameter("avgSpeed", value.AvgSpeed),
                new NpgsqlParameter("breaks", value.Breaks),
                new NpgsqlParameter("brawls", value.Brawls),
                new NpgsqlParameter("abductions", value.Abductions),
                new NpgsqlParameter("hobgoblinSightings", value.HobgoblinSightings),
                new NpgsqlParameter("ufoSightings", value.UFOSightings),
                new NpgsqlParameter("totalTime", value.TotalTime),
                new NpgsqlParameter("rating", value.Rating),
                new NpgsqlParameter("tourlogID", value.ID),
            };

            database.ExecuteNonQuery(statement, transaction, parameters);
        }

        public void Delete(TourLog value, NpgsqlTransaction transaction = null)
        {
            Assert.NotNull(value, nameof(value));

            const string statement = @"DELETE FROM public.""TourLog""
                WHERE ""TourLog_ID"" = @tourlogID;";

            database.ExecuteNonQuery(statement, transaction, new NpgsqlParameter("tourlogID", value.ID));
        }

        public IEnumerable<TourLog> GetTourLogs(Tour tour, NpgsqlTransaction transaction = null)
        {
            return GetTourLogsWhere("\"Tour_ID\" = @tourID", transaction, new NpgsqlParameter("tourID", tour.ID));
        }

        private IEnumerable<TourLog> GetTourLogsWhere(string whereCondition, NpgsqlTransaction transaction = null, params NpgsqlParameter[] parameters)
        {
            const string statement = @"SELECT ""TourLog_ID"", ""Tour_ID"", ""TourDate"",
                ""Distance"", ""AvgSpeed"", ""Breaks"", ""Brawls"", ""Abductions"",
                ""HobgoblinSightings"", ""UFOSightings"", ""TotalTime"", ""rating""
                FROM public.""TourLog""";

            StringBuilder sql = new StringBuilder(statement);

            if (!string.IsNullOrWhiteSpace(whereCondition))
            {
                sql.Append(" WHERE ");
                sql.Append(whereCondition);
            }

            sql.Append(";");

            return database.Execute(sql.ToString(), transaction, RowSelector, parameters);
        }

        private static TourLog RowSelector(NpgsqlDataReader reader)
        {
            return new TourLog()
            {
                ID = reader.GetValue<int>("TourLog_ID"),
                TourDate = reader.GetValue<DateTime>("TourDate"),
                Distance = decimal.ToDouble(reader.GetValue<decimal>("Distance")),
                AvgSpeed = decimal.ToDouble(reader.GetValue<decimal>("AvgSpeed")),
                Breaks = reader.GetValue<int>("Breaks"),
                Brawls = reader.GetValue<int>("Brawls"),
                Abductions = reader.GetValue<int>("Abductions"),
                HobgoblinSightings = reader.GetValue<int>("HobgoblinSightings"),
                UFOSightings = reader.GetValue<int>("UFOSightings"),
                TotalTime = decimal.ToDouble(reader.GetValue<decimal>("TotalTime")),
                Rating = reader.GetValue<int>("rating")
            };
        }

        private readonly PostgreSqlDatabase database;
    }
}
