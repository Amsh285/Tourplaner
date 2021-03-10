using System;
using System.Collections.Generic;
using System.Text;

namespace Tourplaner.Infrastructure.Logging
{
    public interface ILogger<T>
    {
        void Info(string message);

        void Error(string message);
    }
}
