using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using Tourplaner.Entities;
using Tourplaner.Infrastructure;
using Tourplaner.Infrastructure.Logging;
using Tourplaner.Models;
using Tourplaner.Reports;
using Tourplaner.UI;

namespace Tourplaner
{
    public class TourScreenViewModel : Screen
    {
        public ICollectionView TourView
        {
            get
            {
                return tourView;
            }
            set
            {
                if (tourView != value)
                {
                    tourView = value;
                    NotifyPropertyChanged(nameof(TourView));
                }
            }
        }

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
                    if (selectedTour != null)
                        selectedTour.Reset();

                    selectedTour = value;
                    NotifyPropertyChanged(nameof(SelectedTour));
                    NotifyPropertyChanged(nameof(SelectedTourVisible));
                    NotifyPropertyChanged(nameof(CanShowPDFReport));
                    NotifyPropertyChanged(nameof(CanCopySelectedTour));
                    NotifyPropertyChanged(nameof(CanDeleteTour));
                }
            }
        }

        public string FilterText
        {
            get
            {
                return filterText;
            }
            set
            {
                if (filterText != value)
                {
                    filterText = value;
                    NotifyPropertyChanged(nameof(FilterText));
                }
            }
        }

        public bool SelectedTourVisible => SelectedTour != null;

        public bool CanShowPDFReport => SelectedTour != null;

        public bool CanCopySelectedTour => SelectedTour != null;

        public bool CanDeleteTour => SelectedTour != null;

        public TourScreenViewModel(TourEntity tourEntity, Func<UpdateTourViewModel> updateTourViewModelFactory,
            MessageBoxService messageBox, ILogger<TourScreenViewModel> logger)
            : base("Tour Übersicht")
        {
            Assert.NotNull(tourEntity, nameof(tourEntity));
            Assert.NotNull(updateTourViewModelFactory, nameof(updateTourViewModelFactory));
            Assert.NotNull(messageBox, nameof(messageBox));
            Assert.NotNull(logger, nameof(logger));

            Tours = new ObservableCollection<UpdateTourViewModel>();
            tourSource = new CollectionViewSource();
            UpdateTourView();

            this.tourEntity = tourEntity;
            this.updateTourViewModelFactory = updateTourViewModelFactory;
            this.messageBox = messageBox;
            this.logger = logger;
        }

        public void RefreshTours()
        {
            int? selectedTourID = SelectedTour?.ID;

            try
            {
                IEnumerable<UpdateTourViewModel> result = tourEntity.GetTours()
                    .Select(t =>
                    {
                        UpdateTourViewModel viewModel = updateTourViewModelFactory();
                        viewModel.Model = t;

                        viewModel.RefreshOriginalTour();
                        viewModel.OnTourUpdated += UpdateTourViewModel_OnTourUpdated;

                        return viewModel;
                    });

                Tours = new ObservableCollection<UpdateTourViewModel>(result);
                SelectedTour = null;

                UpdateTourView();
            }
            catch (Exception ex)
            {
                messageBox.ShowError($"Error Refreshing Tours: {ex.Message}");
                logger.Error(ex.Message);
            }

            if(selectedTourID.HasValue)
                SelectedTour = Tours.FirstOrDefault(t => t.ID == selectedTourID.Value);
        }

        public void ShowPDFReport()
        {
            if (CanShowPDFReport)
            {
                const string filePath = "tour_report.pdf";

                TourDocument document = new TourDocument(SelectedTour.Model, SelectedTour.StaticMapImage);
                document.GeneratePdf(filePath);

                Process.Start("explorer.exe", filePath);
            }
        }

        public void CopySelectedTour()
        {
            try
            {
                int copyID = tourEntity.Copy(SelectedTour.Model);
                RefreshTours();

                SelectedTour = Tours.FirstOrDefault(t => t.ID == copyID);
            }
            catch (Exception ex)
            {
                messageBox.ShowError($"Error duplicating Tour: {ex.Message}");
                logger.Error(ex.Message);
            }
        }

        public void DeleteTour()
        {
            try
            {
                tourEntity.DeleteTour(SelectedTour.Model);
                SelectedTour = null;
            }
            catch (Exception ex)
            {
                messageBox.ShowError($"Error deleting Tour: {ex.Message}");
                logger.Error(ex.Message);
            }

            RefreshTours();
        }

        public void HandleFilterTextChanged()
        {
            if (!string.IsNullOrWhiteSpace(FilterText))
                TourView.Filter = new Predicate<object>(FilterTourItem);
            else
                TourView.Filter = null;
        }

        private bool FilterTourItem(object item)
        {
            if (item is UpdateTourViewModel model)
                return model.Name.Contains(FilterText) || (model.From?.Contains(FilterText) ?? false) ||
                    (model.To?.Contains(FilterText) ?? false) || (model.Description?.Contains(FilterText) ?? false);

            return false;
        }

        public void OnOverviewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is UpdateTourViewModel editTourViewModel)
            {
                SelectedTour = editTourViewModel;
                SelectedTour.RefreshMapImage();
            }
        }

        private void UpdateTourViewModel_OnTourUpdated(object sender, EventArgs e)
        {
            RefreshTours();
        }

        private void UpdateTourView()
        {
            tourSource.Source = Tours;
            TourView = tourSource.View;
        }

        private ICollectionView tourView;
        private CollectionViewSource tourSource;
        private ObservableCollection<UpdateTourViewModel> tours;
        private UpdateTourViewModel selectedTour;

        private string filterText;

        private readonly TourEntity tourEntity;
        private readonly Func<UpdateTourViewModel> updateTourViewModelFactory;
        private readonly MessageBoxService messageBox;
        private readonly ILogger<TourScreenViewModel> logger;
    }
}
