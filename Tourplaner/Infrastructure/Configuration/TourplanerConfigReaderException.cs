using System;

namespace Tourplaner.Infrastructure.Configuration
{
    public sealed class TourplanerConfigReaderException : Exception
    {
        public TourplanerConfigReaderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
