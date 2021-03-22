using System;
using System.Collections.Generic;
using System.Text;
using Tourplaner.Infrastructure;

namespace Tourplaner
{
    public sealed class CreateTourScreenViewModel : EditTourViewModel, IScreen
    {
        public bool CanSaveTour
        {
            get
            {
                return canSaveTour;
            }
            set
            {
                if (canSaveTour != value)
                {
                    canSaveTour = value;
                    NotifyPropertyChanged(nameof(CanSaveTour));
                }
            }
        }

        public string DisplayName => "Create Tour";

        private bool canSaveTour;
    }
}
