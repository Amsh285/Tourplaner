using System;
using System.Collections.Generic;
using System.Linq;

namespace Tourplaner.Infrastructure
{
    public class CopyNameSelector
    {
        public string GetCopyName(string originalName, IEnumerable<string> names, int maxLength = 100)
        {
            const string copySuffix = "_copy_";

            Assert.NotNull(originalName, nameof(originalName));
            Assert.NotNull(names, nameof(names));

            string tourNameWithCopySuffix = $"{originalName}{copySuffix}";

            IEnumerable<string> iterators = names
                .Where(n => n.StartsWith(tourNameWithCopySuffix))
                .Select(n => n.Replace(tourNameWithCopySuffix, string.Empty));

            int GetIterator(string value)
            {
                if (int.TryParse(value, out int iterator))
                    return iterator;

                return 0;
            }

            int maxIterator = iterators
                .Select(GetIterator)
                .MaxOrDefault();

            if (maxIterator < 0)
                throw new InvalidOperationException($"Invalid Iterator: {maxIterator}. Iterator must be positive");

            if (maxIterator == int.MaxValue)
                throw new InvalidOperationException($"Invalid Iterator: {maxIterator}. Iterator cannot bei larger than: {int.MaxValue}");

            string fullTourName = $"{tourNameWithCopySuffix}{++maxIterator}";

            if (fullTourName.Length > maxLength)
                throw new InvalidOperationException(
                    $"Invalid Copyname: {fullTourName}. " +
                    $"Copyname must be smaller or equal to {maxLength} character signs."
            );

            return fullTourName;
        }
    }
}
