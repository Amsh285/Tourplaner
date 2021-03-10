using System;
using System.Collections.Generic;
using System.Text;
using Tourplaner.Infrastructure;

namespace Tourplaner.UI
{
    public sealed class TabItemViewModel
    {
        public string Header { get; set; }

        public PropertyChangedBase ViewModel { get; set; }
    }
}
