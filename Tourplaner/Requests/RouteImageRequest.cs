using System;
using System.Net.Http;
using System.Threading.Tasks;
using Tourplaner.Infrastructure;
using Tourplaner.Models;

namespace Tourplaner.Requests
{
    public sealed class RouteImageRequest
    {
        public byte[] GetStaticMapImage(RouteInformation routeInformation)
        {
            Task<byte[]> requestTask = RequestStaticMapImage(routeInformation);
            requestTask.Wait();

            return requestTask.Result;
        }

        private async Task<byte[]> RequestStaticMapImage(RouteInformation routeInformation)
        {
            Assert.NotNull(routeInformation, nameof(routeInformation));
            string routeType = Enum.GetName(typeof(RouteType), routeInformation.RouteType);

            string staticMapUrl = $"http://www.mapquestapi.com/staticmap/v5/map?key=RwzmiyOYGW0yRqM4gFEdfJ6UwySfSHLE&start={routeInformation.From}&end={routeInformation.To}&outFormat=json&ambiguities=ignore&routeType={routeType}&doReverseGeocode=false&enhancedNarrative=false&avoidTimedConditions=false";

            Uri requestUri = new Uri(staticMapUrl);
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(5);

            Task<HttpResponseMessage> response = client.GetAsync(requestUri);
            response.Wait();
            response.Result.EnsureSuccessStatusCode();

            return await response.Result.Content.ReadAsByteArrayAsync();
        }
    }
}
