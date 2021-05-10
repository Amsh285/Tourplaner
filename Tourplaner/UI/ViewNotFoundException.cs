using System;

namespace Tourplaner.UI
{
    public sealed class ViewNotFoundException : Exception
    {
        public ViewNotFoundException(string message)
            : base(message)
        {
        }
    }
}
