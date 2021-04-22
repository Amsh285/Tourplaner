using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Tourplaner.Infrastructure.Data;
using Tourplaner.Models;

namespace Tourplaner.Tests
{
    public class DatasetPatcherTests
    {
        [SetUp]
        public void Setup()
        {
            this.newState = new List<TourLog>()
            {
                new TourLog() { ID = 1, Abductions = 1 },
                new TourLog() { ID = 2, Abductions = 3 },
                new TourLog() { ID = 3, Abductions = 450 },
                new TourLog() { ID = 5, Abductions = 42 },
            };

            this.oldState = new List<TourLog>()
            {
                new TourLog() { ID = 1, Abductions = 1 },
                new TourLog() { ID = 2, Abductions = 3 },
                new TourLog() { ID = 3, Abductions = 3 },
                new TourLog() { ID = 4, Abductions = 7 },
            };

            this.unitOfWork = DatasetPatcher.PatchRows<TourLog, int>(newState, oldState);
        }

        [Test]
        public void NewIDsAreMarkedForInsert()
        {
            Assert.IsNotNull(unitOfWork.RowsToInsert.FirstOrDefault(n => n.ID == 5));
            Assert.IsNull(unitOfWork.RowsToUpdate.FirstOrDefault(n => n.ID == 5));
            Assert.IsNull(unitOfWork.RowsToDelete.FirstOrDefault(n => n.ID == 5));
        }

        [Test]
        public void MatchingIDsAreMarkedForUpdate()
        {
            Assert.IsNull(unitOfWork.RowsToInsert.FirstOrDefault(n => n.ID == 2));
            Assert.IsNotNull(unitOfWork.RowsToUpdate.FirstOrDefault(n => n.ID == 2));
            Assert.IsNull(unitOfWork.RowsToDelete.FirstOrDefault(n => n.ID == 2));

            Assert.IsNull(unitOfWork.RowsToInsert.FirstOrDefault(n => n.ID == 3));
            Assert.IsNotNull(unitOfWork.RowsToUpdate.FirstOrDefault(n => n.ID == 3));
            Assert.IsNull(unitOfWork.RowsToDelete.FirstOrDefault(n => n.ID == 3));
        }

        [Test]
        public void MatchingIDsReturningNewValuesForUpdate()
        {
            TourLog updateEntry = unitOfWork.RowsToUpdate.First(n => n.ID == 3);
            Assert.AreEqual(updateEntry.Abductions, 450);
        }

        [Test]
        public void MissingEntriesAreMarkedForDelete()
        {
            Assert.IsNull(unitOfWork.RowsToInsert.FirstOrDefault(n => n.ID == 4));
            Assert.IsNull(unitOfWork.RowsToUpdate.FirstOrDefault(n => n.ID == 4));
            Assert.IsNotNull(unitOfWork.RowsToDelete.FirstOrDefault(n => n.ID == 4));
        }

        private List<TourLog> newState;
        private List<TourLog> oldState;
        private DatasetUnitOfWork<TourLog> unitOfWork;
    }
}