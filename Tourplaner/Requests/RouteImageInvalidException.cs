using System;

namespace Tourplaner.Requests
{
    public sealed class RouteImageInvalidException : Exception
    {
        public RouteImageInvalidException(string message)
            : base(message)
        {
        }
    }
}
