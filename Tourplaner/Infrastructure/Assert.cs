using System;
using System.Collections.Generic;
using System.Text;

namespace Tourplaner.Infrastructure
{
    public static class Assert
    {
        public static void NotNull<T>(T value, string name) where T : class
        {
            That(value != null, $"{name} cannot be null.");
        }

        public static void That(bool condition, string errorMessage)
        {
            if (!condition)
                throw new AssertionException(errorMessage);
        }
    }
}
