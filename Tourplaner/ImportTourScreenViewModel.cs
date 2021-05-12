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
    public class ImportTourScreenViewModel : PropertyChangedBase, IScreen
    {
        public string DisplayName => "Import Tours";

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

        //public bool CanImport => Tours.Any(t => t.IsMarkedForImport);

        public ImportTourScreenViewModel(TourEntity tourEntity, MessageBoxService messageBox, ILogger<ImportTourScreenViewModel> logger)
        {
            Assert.NotNull(tourEntity, nameof(tourEntity));
            Assert.NotNull(messageBox, nameof(messageBox));
            Assert.NotNull(logger, nameof(logger));

            this.tourEntity = tourEntity;
            this.messageBox = messageBox;
            this.logger = logger;

            TourSelectionScreenViewModel = new TourSelectionScreenViewModel() { DisplayName = "Import Tours" };
        }

        public void LoadTours()
        {
            try
            {
                IReadOnlyList<Tour> storedTours = tourEntity
                    .GetTours()
                    .ToList();

                OpenFileDialog dialog = new OpenFileDialog();
                dialog.DefaultExt = ".json";
                dialog.Filter = "Javascript Object Notation (.json)|*.json";

                if (dialog.ShowDialog() == true)
                {
                    using (FileStream stream = File.OpenRead(dialog.FileName))
                    {
                        ValueTask<List<Tour>> loadTours = JsonSerializer.DeserializeAsync<List<Tour>>(stream);
                        loadTours.AsTask().Wait();

                        TourSelectionScreenViewModel.Tours = loadTours.Result
                            .Select(t => new TourSelectionViewModel() { Model = t })
                            .ToObservableCollection();
                    }
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowError($"Tour could not be loaded: {ex.Message}");
                logger.Error(ex.Message);
            }
        }

        private TourSelectionScreenViewModel tourSelectionScreenViewModel;

        private readonly TourEntity tourEntity;
        private readonly MessageBoxService messageBox;
        private readonly ILogger<ImportTourScreenViewModel> logger;
    }
}
