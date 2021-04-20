using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Tourplaner.Entities;
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

        public TourScreenViewModel(TourEntity tourEntity, Func<EditTourViewModel> editTourViewModelFactory, ILogger<TourScreenViewModel> logger)
            : base("Tour Übersicht")
        {
            Assert.NotNull(tourEntity, nameof(tourEntity));
            Assert.NotNull(editTourViewModelFactory, nameof(editTourViewModelFactory));
            Assert.NotNull(logger, nameof(logger));

            Tours = new ObservableCollection<EditTourViewModel>();

            this.tourEntity = tourEntity;
            this.editTourViewModelFactory = editTourViewModelFactory;
            this.logger = logger;
        }

        public void RefreshTours()
        {
            try
            {
                IEnumerable<EditTourViewModel> result = tourEntity.GetTours()
                    .Select(t => {
                        EditTourViewModel viewModel = editTourViewModelFactory();
                        viewModel.Model = t;

                        return viewModel;
                    });

                Tours = new ObservableCollection<EditTourViewModel>(result);
                SelectedTour = null;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
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

        private readonly TourEntity tourEntity;
        private readonly Func<EditTourViewModel> editTourViewModelFactory;
        private readonly ILogger<TourScreenViewModel> logger;
    }
}
