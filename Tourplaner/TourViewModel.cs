using System;
using System.Collections.Generic;
using System.Text;
using Tourplaner.Infrastructure;
using Tourplaner.Models;

namespace Tourplaner
{
    public class TourViewModel : ViewModel<Tour>
    {
        public TourViewModel()
        {
            this.Model = new Tour();
        }
    }
}
