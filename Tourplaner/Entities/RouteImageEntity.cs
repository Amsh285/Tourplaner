using System;
using System.Collections.Generic;
using System.Text;
using Tourplaner.Infrastructure;
using Tourplaner.Models;
using Tourplaner.Repositories;
using Tourplaner.Requests;

namespace Tourplaner.Entities
{
    public sealed class RouteImageEntity
    {
        public RouteImageEntity(RouteImageRepository routeImageRepository, RouteImageRequest routeImageRequest)
        {
            Assert.NotNull(routeImageRepository, nameof(routeImageRepository));
            Assert.NotNull(routeImageRequest, nameof(routeImageRequest));

            this.routeImageRepository = routeImageRepository;
            this.routeImageRequest = routeImageRequest;
        }

        public byte[] GetRouteImage(RouteInformation routeInformation)
        {
            Assert.NotNull(routeInformation, nameof(routeInformation));

            if (routeImageRepository.Exists(routeInformation))
                return routeImageRepository.GetRouteImage(routeInformation);

            byte[] routeImage = routeImageRequest.GetStaticMapImage(routeInformation);

            if (routeImage == null)
                throw new RouteImageInvalidException($"Webservice returned no Routeimage.");

            routeImageRepository.SaveRouteImage(routeInformation, routeImage);
            return routeImage;
        }

        private readonly RouteImageRepository routeImageRepository;
        private readonly RouteImageRequest routeImageRequest;
    }
}
