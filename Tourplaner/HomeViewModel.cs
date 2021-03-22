using System;
using System.Collections.Generic;
using System.Text;
using Tourplaner.Infrastructure;

namespace Tourplaner
{
    public class HomeViewModel : PropertyChangedBase, IScreen
    {
        public string Lol { get; set; }

        public string DisplayName => "Test";
    }
}
