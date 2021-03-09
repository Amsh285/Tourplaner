namespace Tourplaner.Models
{
    public sealed class Tour
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public RouteInformation Route { get; set; }
    }
}
