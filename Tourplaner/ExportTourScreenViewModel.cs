using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Data;
using Tourplaner.Entities;
using Tourplaner.Infrastructure;
using Tourplaner.Infrastructure.Logging;
using Tourplaner.Models;
using Tourplaner.UI;
using Tourplaner.ViewModels;

namespace Tourplaner
{
    public class ExportTourScreenViewModel : PropertyChangedBase, IScreen
    {
        public string DisplayName => "Export Tours";

        public ICollectionView ExportTourView
        {
            get
            {
                return exportTourView;
            }
            set
            {
                if (exportTourView != value)
                {
                    exportTourView = value;
                    NotifyPropertyChanged(nameof(ExportTourView));
                }
            }
        }

        public ObservableCollection<ExportTourViewModel> Tours
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
                    NotifyPropertyChanged(nameof(CanExportTours));
                }
            }
        }

        public bool CheckAllChecked
        {
            get
            {
                return checkAllChecked;
            }
            set
            {
                if (checkAllChecked != value)
                {
                    checkAllChecked = value;
                    NotifyPropertyChanged(nameof(CheckAllChecked));
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

        public bool CanExportTours => Tours.Any(t => t.IsMarkedForExport);

        public ExportTourScreenViewModel(TourEntity tourEntity, MessageBoxService messageBox, ILogger<ExportTourScreenViewModel> logger)
        {
            Assert.NotNull(tourEntity, nameof(tourEntity));
            Assert.NotNull(messageBox, nameof(messageBox));
            Assert.NotNull(logger, nameof(logger));

            this.tourEntity = tourEntity;
            this.messageBox = messageBox;
            this.logger = logger;

            this.Tours = new ObservableCollection<ExportTourViewModel>();
            this.exportTourSource = new CollectionViewSource();
        }

        public void RefreshTours()
        {
            try
            {
                IEnumerable<ExportTourViewModel> result = tourEntity.GetTours()
                    .Select(t =>
                        {
                            ExportTourViewModel viewModel = new ExportTourViewModel() { Model = t };
                            viewModel.PropertyChanged += ExportTourViewModelPropertyChanged;

                            return viewModel;
                        }
                    );

                Tours = new ObservableCollection<ExportTourViewModel>(result);
                UpdateTourView();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        public void ApplyCheckAll()
        {
            foreach (ExportTourViewModel tour in Tours)
                tour.IsMarkedForExport = CheckAllChecked;
        }

        public void ApplyUncheckAll()
        {
            if (Tours.All(t => t.IsMarkedForExport))
                ApplyCheckAll();
        }

        public void ExportMarkedTours()
        {
            try
            {
                if (CanExportTours)
                {
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.FileName = "TourExport";
                    dialog.DefaultExt = ".json";
                    dialog.Filter = "Javascript Object Notation (.json)|*.json";

                    if (dialog.ShowDialog() == true)
                    {
                        List<Tour> markedTours = Tours
                            .Where(t => t.IsMarkedForExport)
                            .Select(t => t.Model)
                            .ToList();

                        JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };

                        using (StreamWriter writer = File.CreateText(dialog.FileName))
                        {
                            Task result = JsonSerializer.SerializeAsync(writer.BaseStream, markedTours, markedTours.GetType(), options);
                            result.Wait();

                            messageBox.ShowInfo("Tour Export erfolgreich.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowError($"Fehler beim Tour Export: {ex.Message}");
                logger.Error(ex.Message);
            }
        }

        public void HandleFilterTextChanged()
        {
            if (!string.IsNullOrWhiteSpace(FilterText))
                ExportTourView.Filter = new Predicate<object>(FilterTourItem);
            else
                ExportTourView.Filter = null;
        }

        private bool FilterTourItem(object item)
        {
            if (item is ExportTourViewModel model)
                return model.Name.Contains(FilterText) || (model.From?.Contains(FilterText) ?? false) ||
                    (model.To?.Contains(FilterText) ?? false) || model.TourLogCount.ToString().Contains(FilterText) ||
                    model.SelectedRouteType.ToString().Contains(filterText);


            return false;
        }

        private void ExportTourViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(ExportTourViewModel.IsMarkedForExport), StringComparison.Ordinal))
            {
                NotifyPropertyChanged(nameof(CanExportTours));

                if (Tours.All(t => t.IsMarkedForExport))
                    CheckAllChecked = true;
                else if (sender is ExportTourViewModel current && !current.IsMarkedForExport)
                    CheckAllChecked = false;
            }
        }

        private void UpdateTourView()
        {
            exportTourSource.Source = Tours;
            ExportTourView = exportTourSource.View;
        }

        private ICollectionView exportTourView;
        private CollectionViewSource exportTourSource;
        private ObservableCollection<ExportTourViewModel> tours;
        private bool checkAllChecked;
        private string filterText;

        private readonly TourEntity tourEntity;
        private readonly MessageBoxService messageBox;
        private readonly ILogger<ExportTourScreenViewModel> logger;
    }
}
