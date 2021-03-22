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

        //public void Insert(TourLog value, NpgsqlTransaction transaction = null)
        //{
        //    const string statement = @"INSERT INTO public.""TourLog""(
        //        ""Tour_ID"", ""TourDate"", ""Distance"", ""AvgSpeed"", ""Breaks"", ""Brawls"", ""Abductions"",
        //        ""HobgoblinSightings"", ""UFOSightings"", ""TotalTime"", ""rating"")
	       //     VALUES(@, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
        //        RETURNING ""TourLog_ID"";";
        //}

        private readonly PostgreSqlDatabase database;
    }
}
