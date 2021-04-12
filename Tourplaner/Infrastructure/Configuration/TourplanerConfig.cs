namespace Tourplaner.Infrastructure.Configuration
{
    public sealed class TourplanerConfig
    {
        public DatabaseSettings DbSettings { get; set; }

        public RouteImageStorageSettings RouteImageSettings { get; set; }
    }
}
