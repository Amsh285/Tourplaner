using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Tourplaner.Infrastructure;

namespace Tourplaner
{
    public class ShellViewModel : PropertyChangedBase
    {
        public ObservableCollection<Tourplaner.UI.TabItem> Tabs
        {
            get
            {
                return tabs;
            }
            set
            {
                if (tabs != value)
                {
                    tabs = value;
                    NotifyPropertyChanged(nameof(Tabs));
                }
            }
        }

        public object CurrentViewModel
        {
            get
            {
                return currentViewModel;
            }
            set
            {
                if (currentViewModel != value)
                {
                    currentViewModel = value;
                    NotifyPropertyChanged(nameof(CurrentViewModel));
                }
            }
        }

        public ShellViewModel()
        {
            Tabs = new ObservableCollection<UI.TabItem>();

            TourViewModel tourViewModel = new TourViewModel();
            //tourViewModel.Model.Description = "Test123";

            CurrentViewModel = new TourViewModel();

            Tabs.Add(new UI.TabItem() { Header = "Tour- Übersicht", ViewModel = tourViewModel });
        }

        private object currentViewModel;
        private ObservableCollection<Tourplaner.UI.TabItem> tabs;
    }
}
