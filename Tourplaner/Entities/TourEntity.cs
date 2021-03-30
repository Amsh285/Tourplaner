using Npgsql;
using Tourplaner.Infrastructure;
using Tourplaner.Infrastructure.Database;
using Tourplaner.Models;
using Tourplaner.Repositories;

namespace Tourplaner.Entities
{
    public sealed class TourEntity
    {
        public TourEntity(PostgreSqlDatabase database, TourRepository tourRepository, TourLogRepository tourLogRepository)
        {
            Assert.NotNull(database, nameof(database));
            Assert.NotNull(tourRepository, nameof(tourRepository));
            Assert.NotNull(tourLogRepository, nameof(tourLogRepository));

            this.database = database;
            this.tourRepository = tourRepository;
            this.tourLogRepository = tourLogRepository;
        }

        public void CreateTour(Tour value)
        {
            using (NpgsqlConnection connection = database.CreateAndOpenConnection())
            using (NpgsqlTransaction transaction = connection.BeginTransaction())
            {
                int tourID = tourRepository.Insert(value, transaction);

                foreach (TourLog log in value.Logs)
                {
                    tourLogRepository.Insert(log, tourID, transaction);
                }

                transaction.Commit();
            }
        }

        private readonly PostgreSqlDatabase database;
        private readonly TourRepository tourRepository;
        private readonly TourLogRepository tourLogRepository;
    }
}
