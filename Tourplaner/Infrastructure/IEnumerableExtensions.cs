using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Tourplaner.Infrastructure
{
    public static class IEnumerableExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return new ObservableCollection<T>(source);
        }
    }
}
