using System;
using System.Collections.Generic;
using System.Text;

namespace Tourplaner.Infrastructure.Data
{
    public interface IIdentity<T> where T : IEquatable<T>
    {
        public T ID { get; }
    }
}
