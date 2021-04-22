using Npgsql;
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
        public event EventHandler OnTourUpdated;

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
            try
            {
                tourEntity.UpdateTour(this.Model);
                messageBox.ShowInfo("Tour updated.");

                originalTour = Model.Copy();
                NotifyPropertyChanged(string.Empty);

                OnTourUpdated(this, EventArgs.Empty);
            }
            catch (PostgresException pex)
            {
                //Todo: Enum!!!
                if (pex.SqlState == "23505")
                    messageBox.ShowInfo($"Tourname: {this.Name} is already in use.", "Invalid Tourname");
                else
                {
                    messageBox.ShowInfo($"{pex.Message}", "Database Error");
                    logger.Info(pex.Message);
                }
            }
            catch (NpgsqlException nex)
            {
                if (nex.InnerException != null)
                    messageBox.ShowInfo(nex.InnerException.Message, "Database Server Error");
                else
                    messageBox.ShowInfo(nex.Message, "Database Server Error");
            }
            catch (Exception ex)
            {
                messageBox.ShowError(ex);
                logger.Error($"Unexpected Error while creating new Tour: {ex.Message}");
            }
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
