using System;
using System.Collections.Generic;
using System.Text;

namespace Tourplaner.UI
{
    public sealed class TabItem
    {
        public string Header { get; set; }

        public object ViewModel { get; set; }
    }
}
