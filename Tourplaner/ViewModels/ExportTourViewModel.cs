using Tourplaner.Models;

namespace Tourplaner.ViewModels
{
    public sealed class ExportTourViewModel : TourViewModel
    {
        public bool IsMarkedForExport
        {
            get
            {
                return isMarkedForExport;
            }
            set
            {
                if (isMarkedForExport != value)
                {
                    isMarkedForExport = value;
                    NotifyPropertyChanged(nameof(IsMarkedForExport));
                }
            }
        }

        public ExportTourViewModel()
        {
            Model = new Tour();
        }

        private bool isMarkedForExport;
    }
}
