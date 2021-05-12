using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
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

        public TourSelectionScreenViewModel TourSelectionScreenViewModel
        {
            get
            {
                return tourSelectionScreenViewModel;
            }
            set
            {
                if (tourSelectionScreenViewModel != value)
                {
                    tourSelectionScreenViewModel = value;
                    NotifyPropertyChanged(nameof(TourSelectionScreenViewModel));
                }
            }
        }

        public bool CanExportTours => TourSelectionScreenViewModel.Tours.Any(t => t.IsMarked) &&
            TourSelectionScreenViewModel.AllMarkedToursValid;

        public ExportTourScreenViewModel(TourEntity tourEntity, MessageBoxService messageBox, ILogger<ExportTourScreenViewModel> logger)
        {
            Assert.NotNull(tourEntity, nameof(tourEntity));
            Assert.NotNull(messageBox, nameof(messageBox));
            Assert.NotNull(logger, nameof(logger));

            this.tourEntity = tourEntity;
            this.messageBox = messageBox;
            this.logger = logger;

            this.TourSelectionScreenViewModel = new TourSelectionScreenViewModel();
            this.TourSelectionScreenViewModel.TourSelectionViewModelChanged += TourSelectionScreenViewModelTourChanged;
        }

        public void RefreshTours()
        {
            try
            {
                IEnumerable<TourSelectionViewModel> result = tourEntity.GetTours()
                    .Select(t => new TourSelectionViewModel() { Model = t });

                TourSelectionScreenViewModel.Tours = result
                    .ToObservableCollection();
            }
            catch (Exception ex)
            {
                messageBox.ShowError($"Error Loading Tourdata: {ex.Message}");
                logger.Error(ex.Message);
            }
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
                        List<Tour> markedTours = TourSelectionScreenViewModel
                            .MarkedTours
                            .Select(t => t.Model)
                            .ToList();

                        JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };

                        using (StreamWriter writer = File.CreateText(dialog.FileName))
                        {
                            Task result = JsonSerializer.SerializeAsync(writer.BaseStream, markedTours, markedTours.GetType(), options);
                            result.Wait();

                            messageBox.ShowInfo("Tour Export Successful.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowError($"Error while exporting Tours: {ex.Message}");
                logger.Error(ex.Message);
            }
        }

        private void TourSelectionScreenViewModelTourChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(nameof(CanExportTours));
        }

        private TourSelectionScreenViewModel tourSelectionScreenViewModel;

        private readonly TourEntity tourEntity;
        private readonly MessageBoxService messageBox;
        private readonly ILogger<ExportTourScreenViewModel> logger;
    }
}
