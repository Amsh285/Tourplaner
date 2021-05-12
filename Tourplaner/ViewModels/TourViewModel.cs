using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Tourplaner.Infrastructure;
using Tourplaner.Models;

namespace Tourplaner.ViewModels
{
    public class TourViewModel : ValidatedViewModel<Tour>
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

        public int TourLogCount => Logs.Count();

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
                    NotifyPropertyChanged(nameof(TourLogCount));
                }
            }
        }

        public TourViewModel()
        {
            this.Model = new Tour();
        }
    }
}
