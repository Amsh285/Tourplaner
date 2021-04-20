using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tourplaner.Entities;
using Tourplaner.Infrastructure;
using Tourplaner.Infrastructure.Logging;
using Tourplaner.Models;
using Tourplaner.UI;

namespace Tourplaner
{
    public class EditTourViewModel : ValidatedViewModel<Tour>
    {
        public int ID
        {
            get
            {
                return Model.ID;
            }
            set
            {
                if (Model.ID != value)
                {
                    Model.ID = value;
                    NotifyPropertyChanged(nameof(ID));
                }
            }
        }

        [Required(ErrorMessage = "Tour must have a Name.")]
        public string Name
        {
            get
            {
                return Model.Name;
            }
            set
            {
                if (Model.Name != value)
                {
                    Model.Name = value;
                    NotifyPropertyChanged(nameof(Name));
                    NotifyPropertyChanged(nameof(IsValid));
                }
            }
        }

        public string Description
        {
            get
            {
                return Model.Description;
            }
            set
            {
                if (Model.Description != value)
                {
                    Model.Description = value;
                    NotifyPropertyChanged(nameof(Description));
                }
            }
        }

        public string From
        {
            get
            {
                return Model.Route.From;
            }
            set
            {
                if (Model.Route.From != value)
                {
                    Model.Route.From = value;
                    NotifyPropertyChanged(nameof(From));
                    NotifyPropertyChanged(nameof(CanRefreshMapImage));
                }
            }
        }

        public string To
        {
            get
            {
                return Model.Route.To;
            }
            set
            {
                if (Model.Route.To != value)
                {
                    Model.Route.To = value;
                    NotifyPropertyChanged(nameof(To));
                    NotifyPropertyChanged(nameof(CanRefreshMapImage));
                }
            }
        }

        public IEnumerable<RouteType> RouteTypes
        {
            get
            {
                return Enum.GetValues(typeof(RouteType))
                    .Cast<RouteType>();
            }
        }

        public RouteType SelectedRouteType
        {
            get
            {
                return Model.Route.RouteType;
            }
            set
            {
                if (Model.Route.RouteType != value)
                {
                    Model.Route.RouteType = value;
                    NotifyPropertyChanged(nameof(SelectedRouteType));
                }
            }
        }

        public ObservableCollection<TourLog> Logs
        {
            get
            {
                return Model.Logs;
            }
            set
            {
                if (Model.Logs != value)
                {
                    Model.Logs = value;
                    NotifyPropertyChanged(nameof(Logs));
                }
            }
        }

        public byte[] StaticMapImage
        {
            get
            {
                return staticMapImage;
            }
            set
            {
                if (staticMapImage != value)
                {
                    staticMapImage = value;
                    NotifyPropertyChanged(nameof(StaticMapImage));
                }
            }
        }

        public bool CanRefreshMapImage => !string.IsNullOrWhiteSpace(From) && !string.IsNullOrWhiteSpace(To);

        public EditTourViewModel(RouteImageEntity routeImageEntity, MessageBoxService messageBox, ILogger<EditTourViewModel> logger)
        {
            Assert.NotNull(routeImageEntity, nameof(routeImageEntity));
            Assert.NotNull(messageBox, nameof(messageBox));
            Assert.NotNull(logger, nameof(logger));

            this.Model = new Tour();
            Assert.NotNull(Model.Route, nameof(Model.Route));

            this.routeImageEntity = routeImageEntity;
            this.messageBox = messageBox;
            this.logger = logger;
        }

        public void RefreshMapImage()
        {
            if (CanRefreshMapImage)
            {
                try
                {
                    StaticMapImage = routeImageEntity.GetRouteImage(Model.Route);
                }
                catch (Exception ex)
                {
                    messageBox.ShowError(ex);
                    logger.Error($"Unexpected Error while requesting RouteImage: {ex.Message}");
                }
            }
            else
                StaticMapImage = null;
        }

        private byte[] staticMapImage;

        private readonly RouteImageEntity routeImageEntity;
        private readonly MessageBoxService messageBox;
        private readonly ILogger<EditTourViewModel> logger;
    }
}
