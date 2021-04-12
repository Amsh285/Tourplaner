using System;
using System.IO;
using Tourplaner.Infrastructure;
using Tourplaner.Infrastructure.Configuration;
using Tourplaner.Models;

namespace Tourplaner.Repositories
{
    public sealed class RouteImageRepository
    {
        public RouteImageRepository(RouteImageStorageSettings settings)
        {
            Assert.NotNull(settings, nameof(settings));

            this.settings = settings;
        }

        public bool Exists(RouteInformation routeInformation)
        {
            Assert.NotNull(routeInformation, nameof(routeInformation));
            string path = GetRouteImagePath(routeInformation);

            return File.Exists(path);
        }

        public void SaveRouteImage(RouteInformation routeInformation, byte[] routeImage)
        {
            Assert.NotNull(routeInformation, nameof(routeInformation));
            string path = GetRouteImagePath(routeInformation);

            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllBytes(path, routeImage);
        }

        public byte[] GetRouteImage(RouteInformation routeInformation)
        {
            Assert.NotNull(routeInformation, nameof(routeInformation));
            string path = GetRouteImagePath(routeInformation);

            return File.ReadAllBytes(path);
        }

        private string GetRouteImagePath(RouteInformation routeInformation)
        {
            return Path.Combine(
                settings.Location,
                Enum.GetName(typeof(RouteType), routeInformation.RouteType),
                $"{routeInformation.From}_{routeInformation.To}.jpg"
            );
        }

        private readonly RouteImageStorageSettings settings;
    }
}
