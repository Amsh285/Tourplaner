namespace Tourplaner.Models
{
    public sealed class Tour
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public RouteInformation Route { get; set; }

        public Tour()
        {
            Route = new RouteInformation();
        }
    }
}
