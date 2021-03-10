using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using Tourplaner.Infrastructure;

namespace Tourplaner
{
    public class ShellViewModel : PropertyChangedBase
    {
        public ObservableCollection<Tourplaner.UI.TabItemViewModel> Tabs
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

        public ShellViewModel()
        {
            Tabs = new ObservableCollection<UI.TabItemViewModel>();

            TourViewModel tourViewModel = new TourViewModel();
            //tourViewModel.Model.Description = "Test123";

            Tabs.Add(new UI.TabItemViewModel() { Header = "Tour- Übersicht", ViewModel = tourViewModel });
            Tabs.Add(new UI.TabItemViewModel() { Header = "Test", ViewModel = new TestViewModel() });
        }

        private ObservableCollection<Tourplaner.UI.TabItemViewModel> tabs;
    }
}
