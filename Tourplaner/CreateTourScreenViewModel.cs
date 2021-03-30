using System;
using System.Collections.Generic;
using System.Text;
using Tourplaner.Entities;
using Tourplaner.Infrastructure;
using Tourplaner.Infrastructure.Logging;

namespace Tourplaner
{
    public sealed class CreateTourScreenViewModel : EditTourViewModel, IScreen
    {
        public string DisplayName => "Create Tour";

        public CreateTourScreenViewModel(TourEntity tourEntity, ILogger<CreateTourScreenViewModel> logger)
        {
            Assert.NotNull(tourEntity, nameof(tourEntity));
            Assert.NotNull(logger, nameof(logger));

            this.tourEntity = tourEntity;
            this.logger = logger;
        }

        public void CreateTour()
        {
            try
            {
                tourEntity.CreateTour(this.Model);
            }
            catch (Exception ex)
            {
                logger.Error($"Unexpected Error while creating new Tour: {ex.Message}");
            }
        }

        private readonly TourEntity tourEntity;
        private readonly ILogger<CreateTourScreenViewModel> logger;
    }
}
