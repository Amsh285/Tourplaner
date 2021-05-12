using System;
using System.Collections.Generic;
using System.Text;

namespace Tourplaner.ViewModels
{
    public class TourSelectionViewModel : TourViewModel
    {
        public bool IsMarked
        {
            get
            {
                return isMarked;
            }
            set
            {
                if (isMarked != value)
                {
                    isMarked = value;
                    NotifyPropertyChanged(nameof(IsMarked));
                }
            }
        }
        private bool isMarked;
    }
}
