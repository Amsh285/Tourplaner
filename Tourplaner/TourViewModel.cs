using System;
using System.Collections.Generic;
using System.Text;
using Tourplaner.Infrastructure;
using Tourplaner.Models;

namespace Tourplaner
{
    public sealed class TourViewModel : ViewModel<Tour>
    {
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

        public TourViewModel()
        {
            this.Model = new Tour();
            Assert.NotNull(Model.Route, nameof(Model.Route));
        }
    }
}
