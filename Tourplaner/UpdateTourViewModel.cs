using System;
using System.Collections.Generic;
using System.Text;
using Tourplaner.Entities;
using Tourplaner.Infrastructure;
using Tourplaner.Infrastructure.Logging;
using Tourplaner.Models;
using Tourplaner.UI;

namespace Tourplaner
{
    public sealed class UpdateTourViewModel : EditTourViewModel, IScreen
    {
        public string DisplayName => "Update Tour";

        public UpdateTourViewModel(RouteImageEntity routeImageEntity, TourEntity tourEntity, MessageBoxService messageBoxService,
            ILogger<EditTourViewModel> editTourViewModelLogger, ILogger<UpdateTourViewModel> logger)
            : base(routeImageEntity, messageBoxService, editTourViewModelLogger)
        {
            Assert.NotNull(tourEntity, nameof(tourEntity));
            Assert.NotNull(logger, nameof(logger));

            this.tourEntity = tourEntity;
            this.logger = logger;
        }

        public void UpdateTour()
        {

        }

        public void RefreshOriginalTour()
        {
            originalTour = Model.Copy();
        }

        public void Reset()
        {
            if(originalTour != null)
            {
                this.Model = originalTour.Copy();
                NotifyPropertyChanged(string.Empty);
            }
        }

        private Tour originalTour;

        private readonly TourEntity tourEntity;
        private readonly ILogger<UpdateTourViewModel> logger;
    }
}
