using System;

namespace Tourplaner.Entities
{
    public class TourEntityException : Exception
    {
        public TourEntityException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}
