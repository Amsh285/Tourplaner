using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Tourplaner.Infrastructure;
using Tourplaner.Infrastructure.Logging;
using Tourplaner.Models;

namespace Tourplaner
{
    public class TourScreenViewModel : Screen
    {
        public ObservableCollection<EditTourViewModel> Tours
        {
            get
            {
                return tours;
            }
            set
            {
                if (tours != value)
                {
                    tours = value;
                    NotifyPropertyChanged(nameof(Tours));
                }
            }
        }

        public EditTourViewModel SelectedTour
        {
            get
            {
                return selectedTour;
            }
            set
            {
                if (selectedTour != value)
                {
                    selectedTour = value;
                    NotifyPropertyChanged(nameof(SelectedTour));
                }
            }
        }

        public bool SelectedTourVisible
        {
            get
            {
                return selectedTourVisible;
            }
            set
            {
                if (selectedTourVisible != value)
                {
                    selectedTourVisible = value;
                    NotifyPropertyChanged(nameof(SelectedTourVisible));
                }
            }
        }

        public TourScreenViewModel(ILogger<TourScreenViewModel> logger)
            : base("Tour Übersicht")
        {
            Assert.NotNull(logger, nameof(logger));

            Tours = new ObservableCollection<EditTourViewModel>();

            this.logger = logger;
        }

        public void OnViewLoaded()
        {

        }

        public void OnOverviewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count > 0 && e.AddedItems[0] is EditTourViewModel editTourViewModel)
            {
                SelectedTour = editTourViewModel;
                SelectedTourVisible = true;
            }
        }

        private ObservableCollection<EditTourViewModel> tours;
        private EditTourViewModel selectedTour;

        private bool selectedTourVisible;
        private readonly ILogger<TourScreenViewModel> logger;
    }
}
