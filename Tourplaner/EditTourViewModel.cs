using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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

        public EditTourViewModel(MessageBoxService messageBox, ILogger<EditTourViewModel> logger)
        {
            Assert.NotNull(messageBox, nameof(messageBox));
            Assert.NotNull(logger, nameof(logger));

            this.Model = new Tour();
            Assert.NotNull(Model.Route, nameof(Model.Route));

            this.messageBox = messageBox;
            this.logger = logger;
        }

        public void RefreshMapImage()
        {
            try
            {
                Task<byte[]> requestTask = RequestStaticMapImage();
                requestTask.Wait();

                StaticMapImage = requestTask.Result;
            }
            catch (Exception ex)
            {
                messageBox.ShowError(ex);
                logger.Error($"Unexpected Error while requesting RouteImage: {ex.Message}");
            }
        }

        private async Task<byte[]> RequestStaticMapImage()
        {
            //Denver Boulder

            string routeType = Enum.GetName(typeof(RouteType), SelectedRouteType);

            string staticMapUrl = $"http://www.mapquestapi.com/staticmap/v5/map?key=RwzmiyOYGW0yRqM4gFEdfJ6UwySfSHLE&start={From}&end={To}&outFormat=json&ambiguities=ignore&routeType={routeType}&doReverseGeocode=false&enhancedNarrative=false&avoidTimedConditions=false";

            Uri requestUri = new Uri(staticMapUrl);
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(5);

            Task<HttpResponseMessage> response = client.GetAsync(requestUri);
            response.Wait();

            response.Result.EnsureSuccessStatusCode();

            return await response.Result.Content.ReadAsByteArrayAsync();
        }

        private byte[] staticMapImage;

        private readonly MessageBoxService messageBox;
        private readonly ILogger<EditTourViewModel> logger;
    }
}
