using System;
using System.Collections.Generic;
using System.Text;

namespace Tourplaner.Infrastructure
{
    public sealed class AssertionException : Exception
    {
        public AssertionException(string message)
            : base(message)
        {
        }
    }
}
