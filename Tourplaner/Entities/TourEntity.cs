using Npgsql;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Tourplaner.Infrastructure;
using Tourplaner.Infrastructure.Data;
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
            Assert.NotNull(value, nameof(value));

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

        public void UpdateTour(Tour value)
        {
            Assert.NotNull(value, nameof(value));

            using (NpgsqlConnection connection = database.CreateAndOpenConnection())
            using (NpgsqlTransaction transaction = connection.BeginTransaction())
            {
                tourRepository.Update(value, transaction);
                IEnumerable<TourLog> oldState = tourLogRepository.GetTourLogs(value, transaction);

                DatasetUnitOfWork<TourLog> unitOfWork = DatasetPatcher.PatchRows<TourLog, int>(value.Logs, oldState);
                
                unitOfWork.RowsToInsert
                    .ToList()
                    .ForEach(l => tourLogRepository.Insert(l, value.ID, transaction));

                unitOfWork.RowsToUpdate
                    .ToList()
                    .ForEach(l => tourLogRepository.Update(l, transaction));

                unitOfWork.RowsToDelete
                    .ToList()
                    .ForEach(l => tourLogRepository.Delete(l, transaction));

                transaction.Commit();
            }
        }

        public void DeleteTour(Tour value)
        {
            Assert.NotNull(value, nameof(value));

            using (NpgsqlConnection connection = database.CreateAndOpenConnection())
            using (NpgsqlTransaction transaction = connection.BeginTransaction())
            {
                foreach (TourLog currentTourLog in value.Logs)
                    tourLogRepository.Delete(currentTourLog, transaction);

                tourRepository.Delete(value, transaction);

                transaction.Commit();
            }
        }

        public IEnumerable<Tour> GetTours()
        {
            using (NpgsqlConnection connection = database.CreateAndOpenConnection())
            using (NpgsqlTransaction transaction = connection.BeginTransaction())
            {
                IEnumerable<Tour> tours = tourRepository.GetTours(transaction);

                foreach (Tour currentTour in tours)
                {
                    IEnumerable<TourLog> logEntries = tourLogRepository.GetTourLogs(currentTour, transaction);
                    ObservableCollection<TourLog> logs = new ObservableCollection<TourLog>(logEntries);
                    currentTour.Logs = logs;
                }

                return tours;
            }
        }

        private readonly PostgreSqlDatabase database;
        private readonly TourRepository tourRepository;
        private readonly TourLogRepository tourLogRepository;
    }
}
