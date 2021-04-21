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
        public ObservableCollection<UpdateTourViewModel> Tours
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

        public UpdateTourViewModel SelectedTour
        {
            get
            {
                return selectedTour;
            }
            set
            {
                if (selectedTour != value)
                {
                    if(selectedTour != null)
                        selectedTour.Reset();

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

        public TourScreenViewModel(TourEntity tourEntity, Func<UpdateTourViewModel> updateTourViewModelFactory, ILogger<TourScreenViewModel> logger)
            : base("Tour Übersicht")
        {
            Assert.NotNull(tourEntity, nameof(tourEntity));
            Assert.NotNull(updateTourViewModelFactory, nameof(updateTourViewModelFactory));
            Assert.NotNull(logger, nameof(logger));

            Tours = new ObservableCollection<UpdateTourViewModel>();

            this.tourEntity = tourEntity;
            this.updateTourViewModelFactory = updateTourViewModelFactory;
            this.logger = logger;
        }

        public void RefreshTours()
        {
            try
            {
                IEnumerable<UpdateTourViewModel> result = tourEntity.GetTours()
                    .Select(t => {
                        UpdateTourViewModel viewModel = updateTourViewModelFactory();
                        viewModel.Model = t;

                        viewModel.RefreshOriginalTour();
                        return viewModel;
                    });

                Tours = new ObservableCollection<UpdateTourViewModel>(result);
                SelectedTour = null;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        public void OnOverviewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems.Count > 0 && e.AddedItems[0] is UpdateTourViewModel editTourViewModel)
            {
                SelectedTour = editTourViewModel;
                SelectedTourVisible = true;
                SelectedTour.RefreshMapImage();
            }
        }

        private ObservableCollection<UpdateTourViewModel> tours;
        private UpdateTourViewModel selectedTour;

        private bool selectedTourVisible;

        private readonly TourEntity tourEntity;
        private readonly Func<UpdateTourViewModel> updateTourViewModelFactory;
        private readonly ILogger<TourScreenViewModel> logger;
    }
}
