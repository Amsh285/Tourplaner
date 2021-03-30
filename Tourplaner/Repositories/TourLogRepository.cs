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

        private readonly PostgreSqlDatabase database;
    }
}
