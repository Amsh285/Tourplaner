using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Tourplaner.Infrastructure;
using Tourplaner.Models;

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

        public RouteType RouteTye
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
                    NotifyPropertyChanged(nameof(RouteTye));
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

        public EditTourViewModel()
        {
            this.Model = new Tour();
            Assert.NotNull(Model.Route, nameof(Model.Route));
        }

        public void RefreshMapImage()
        {
            //const string todo = "https://www.mapquestapi.com/staticmap/v5/map?key=RwzmiyOYGW0yRqM4gFEdfJ6UwySfSHLE&start=Denver&end=Boulder&outFormat=json&ambiguities=ignore&routeType=fastest&doReverseGeocode=false&enhancedNarrative=false&avoidTimedConditions=false";

            //const string url = "http://www.mapquestapi.com/directions/v2/route";
            //string queryString = $"key=KEY&from={From}&to={To}&outFormat=json" +
            //    $"&ambiguities=ignore&routeType=fastest&doReverseGeocode=false" +
            //    "&enhancedNarrative=false&avoidTimedConditions=false";

            //string requestUrl = $"{url}?{queryString}";

            //HttpClient client = new HttpClient();

            //try
            //{
            //    Task<HttpResponseMessage> response = client.GetAsync(requestUrl);
            //    response.Wait();


            //}
            //catch (System.Exception)
            //{
            //    //Todo
            //}
        }
    }
}
