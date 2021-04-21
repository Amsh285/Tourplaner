using System.Collections.ObjectModel;
using System.Linq;
using Tourplaner.Infrastructure;

namespace Tourplaner.Models
{
    public sealed class Tour
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public RouteInformation Route { get; set; }

        public ObservableCollection<TourLog> Logs { get; set; }

        public Tour()
        {
            Route = new RouteInformation();
            Logs = new ObservableCollection<TourLog>();
        }

        public Tour Copy()
        {
            RouteInformation routeCopy = new RouteInformation()
            {
                Ambiguities = Route.Ambiguities,
                Format = Route.Format,
                From = Route.From,
                To = Route.To,
                RouteType = Route.RouteType
            };

            ObservableCollection<TourLog> logCopies = Logs
                .Select(l => l.Copy())
                .ToObservableCollection();

            return new Tour()
            {
                ID = ID,
                Name = Name,
                Description = Description,
                Route = routeCopy,
                Logs = logCopies
            };
        }
    }
}
