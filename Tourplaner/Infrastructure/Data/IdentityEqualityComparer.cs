using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Tourplaner.Infrastructure.Data
{
    public sealed class IdentityEqualityComparer<T, TIdentifier> : IEqualityComparer<T>
        where TIdentifier : IEquatable<TIdentifier>
        where T : IIdentity<TIdentifier>
    {
        public bool Equals([AllowNull] T x, [AllowNull] T y)
        {
            return (x == null && y == null) || ((x != null && y != null) && x.ID.Equals(y.ID));
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            return obj.ID.GetHashCode();
        }
    }
}
