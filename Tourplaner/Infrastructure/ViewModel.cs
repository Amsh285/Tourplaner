using System;
using System.Collections.Generic;
using System.Text;

namespace Tourplaner.Infrastructure
{
    public abstract class ViewModel<T> : PropertyChangedBase
    {
        public T Model { get; set; }
    }
}
