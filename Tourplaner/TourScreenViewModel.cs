using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Tourplaner.Infrastructure;
using Tourplaner.Models;

namespace Tourplaner
{
    public class TourScreenViewModel : Screen
    {
        public ObservableCollection<TourOverView> Overviews
        {
            get
            {
                return overviews;
            }
            set
            {
                if (overviews != value)
                {
                    overviews = value;
                    NotifyPropertyChanged(nameof(Overviews));
                }
            }
        }

        public TourOverView SelectedOverView
        {
            get
            {
                return selectedOverview;
            }
            set
            {
                if (selectedOverview != value)
                {
                    selectedOverview = value;
                    NotifyPropertyChanged(nameof(SelectedOverView));
                }
            }
        }

        public TourViewModel SelectedTour
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

                    SelectedTourVisible = value != null;
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

        public TourScreenViewModel()
            : base("Tour Übersicht")
        {
            Overviews = new ObservableCollection<TourOverView>()
            {
                new TourOverView() { ID=123, Name="Xd" },
                new TourOverView() { ID=456, Name="lol" },
                new TourOverView() { ID=789, Name="rofl" }
            };

            NotifyPropertyChanged(nameof(SelectedTourVisible));
        }

        private ObservableCollection<TourOverView> overviews;
        private TourOverView selectedOverview;
        private TourViewModel selectedTour;

        private bool selectedTourVisible;
    }
}
