using System;
using System.Windows;
using System.Windows.Controls;
using Tourplaner.Infrastructure;

namespace Tourplaner
{
    public class ShellViewModel : PropertyChangedBase
    {
        public IScreen SelectedScreen
        {
            get
            {
                return selectedScreen;
            }
            set
            {
                if (selectedScreen != value)
                {
                    selectedScreen = value;
                    NotifyPropertyChanged(nameof(SelectedScreen));
                }
            }
        }

        public Visibility ButtonOpenMenuVisibility
        {
            get
            {
                return buttonOpenMenuVisibility;
            }
            set
            {
                if (buttonOpenMenuVisibility != value)
                {
                    buttonOpenMenuVisibility = value;
                    NotifyPropertyChanged(nameof(ButtonOpenMenuVisibility));
                }
            }
        }

        public Visibility ButtonCloseMenuVisibility
        {
            get
            {
                return buttonCloseMenuVisibility;
            }
            set
            {
                if (buttonCloseMenuVisibility != value)
                {
                    buttonCloseMenuVisibility = value;
                    NotifyPropertyChanged(nameof(ButtonCloseMenuVisibility));
                }
            }
        }

        public ShellViewModel(Func<CreateTourScreenViewModel> createTourScreenViewModel, Func<HomeViewModel> homeViewModel,
            Func<TourScreenViewModel> tourScreenViewModel, Func<ExportTourScreenViewModel> exportTourScreenViewModelFactory,
            Func<ImportTourScreenViewModel> importTourScreenViewModelFactory)
        {
            Assert.NotNull(createTourScreenViewModel, nameof(createTourScreenViewModel));
            Assert.NotNull(homeViewModel, nameof(homeViewModel));
            Assert.NotNull(tourScreenViewModel, nameof(tourScreenViewModel));
            Assert.NotNull(exportTourScreenViewModelFactory, nameof(exportTourScreenViewModelFactory));
            Assert.NotNull(importTourScreenViewModelFactory, nameof(importTourScreenViewModelFactory));

            this.createTourScreenViewModel = createTourScreenViewModel;
            this.homeViewModel = homeViewModel;
            this.tourScreenViewModel = tourScreenViewModel;
            this.exportTourScreenViewModelFactory = exportTourScreenViewModelFactory;
            this.importTourScreenViewModelFactory = importTourScreenViewModelFactory;

            ButtonOpenMenuVisibility = Visibility.Visible;
            ButtonCloseMenuVisibility = Visibility.Collapsed;
        }

        public void OnMenuSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is ListViewItem item)
            {
                if (item.Name.Equals("ItemHome", StringComparison.Ordinal))
                    SelectedScreen = homeViewModel();
                else if (item.Name.Equals("ItemCreate", StringComparison.Ordinal))
                    SelectedScreen = createTourScreenViewModel();
                else if (item.Name.Equals("ItemOverview", StringComparison.Ordinal))
                    SelectedScreen = tourScreenViewModel();
                else if (item.Name.Equals("TourExport", StringComparison.Ordinal))
                    SelectedScreen = exportTourScreenViewModelFactory();
                else if (item.Name.Equals("TourImport", StringComparison.Ordinal))
                    SelectedScreen = importTourScreenViewModelFactory();
            }
        }

        public void OpenMenu(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenuVisibility = Visibility.Visible;
            ButtonOpenMenuVisibility = Visibility.Collapsed;
        }

        public void CloseMenu(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenuVisibility = Visibility.Collapsed;
            ButtonOpenMenuVisibility = Visibility.Visible;
        }

        private Visibility buttonOpenMenuVisibility;
        private Visibility buttonCloseMenuVisibility;
        private IScreen selectedScreen;

        private readonly Func<CreateTourScreenViewModel> createTourScreenViewModel;
        private readonly Func<HomeViewModel> homeViewModel;
        private readonly Func<TourScreenViewModel> tourScreenViewModel;
        private readonly Func<ExportTourScreenViewModel> exportTourScreenViewModelFactory;
        private readonly Func<ImportTourScreenViewModel> importTourScreenViewModelFactory;
    }
}
