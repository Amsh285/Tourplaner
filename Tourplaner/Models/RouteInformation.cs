using System;
using System.Collections.Generic;
using System.Text;

namespace Tourplaner.Models
{
    public sealed class RouteInformation
    {
        public string From { get; set; }

        public string To { get; set; }

        public string Format { get; set; }

        public string Ambiguities { get; set; }

        public string RouteType { get; set; }
    }
}
