using Npgsql;
using System;
using Tourplaner.Entities;
using Tourplaner.Infrastructure;
using Tourplaner.Infrastructure.Logging;
using Tourplaner.UI;

namespace Tourplaner
{
    public sealed class CreateTourScreenViewModel : EditTourViewModel, IScreen
    {
        public string DisplayName => "Create Tour";

        public CreateTourScreenViewModel(TourEntity tourEntity, RouteImageEntity routeImageEntity, MessageBoxService messageBox, ILogger<CreateTourScreenViewModel> logger, ILogger<EditTourViewModel> baseLogger)
            : base(routeImageEntity, messageBox, baseLogger)
        {
            Assert.NotNull(tourEntity, nameof(tourEntity));
            Assert.NotNull(messageBox, nameof(messageBox));
            Assert.NotNull(logger, nameof(logger));

            this.tourEntity = tourEntity;
            this.messageBox = messageBox;
            this.logger = logger;
        }

        public void CreateTour()
        {
            try
            {
                tourEntity.CreateTour(this.Model);
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
            catch(NpgsqlException nex)
            {
                if(nex.InnerException != null)
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

        private readonly TourEntity tourEntity;
        private readonly MessageBoxService messageBox;
        private readonly ILogger<CreateTourScreenViewModel> logger;
    }
}
