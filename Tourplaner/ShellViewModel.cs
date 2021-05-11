using Autofac;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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

        public ShellViewModel(Func<CreateTourScreenViewModel> createTourScreenViewModel, Func<HomeViewModel> homeViewModel,
            Func<TourScreenViewModel> tourScreenViewModel, Func<ExportTourScreenViewModel> exportTourScreenViewModelFactory)
        {
            Assert.NotNull(createTourScreenViewModel, nameof(createTourScreenViewModel));
            Assert.NotNull(homeViewModel, nameof(homeViewModel));
            Assert.NotNull(tourScreenViewModel, nameof(tourScreenViewModel));
            Assert.NotNull(exportTourScreenViewModelFactory, nameof(exportTourScreenViewModelFactory));

            this.createTourScreenViewModel = createTourScreenViewModel;
            this.homeViewModel = homeViewModel;
            this.tourScreenViewModel = tourScreenViewModel;
            this.exportTourScreenViewModelFactory = exportTourScreenViewModelFactory;
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
            }
        }

        private IScreen selectedScreen;
        private readonly Func<CreateTourScreenViewModel> createTourScreenViewModel;
        private readonly Func<HomeViewModel> homeViewModel;
        private readonly Func<TourScreenViewModel> tourScreenViewModel;
        private readonly Func<ExportTourScreenViewModel> exportTourScreenViewModelFactory;
    }
}
