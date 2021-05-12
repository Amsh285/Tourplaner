using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Tourplaner.Infrastructure;
using Tourplaner.ViewModels;

namespace Tourplaner
{
    public class TourSelectionScreenViewModel : PropertyChangedBase, IScreen
    {
        public event PropertyChangedEventHandler TourSelectionViewModelChanged;

        public string DisplayName
        {
            get
            {
                return displayName;
            }
            set
            {
                if (displayName != value)
                {
                    displayName = value;
                    NotifyPropertyChanged(nameof(DisplayName));
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

        public ObservableCollection<TourSelectionViewModel> Tours
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

                    UpdateTourView();
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

        public bool AllToursValid => Tours.All(t => t.IsValid);

        public bool AllMarkedToursValid => MarkedTours.All(t => t.IsValid);

        public IEnumerable<TourSelectionViewModel> MarkedTours => Tours.Where(t => t.IsMarked);

        public TourSelectionScreenViewModel()
        {
            Tours = new ObservableCollection<TourSelectionViewModel>();
        }

        public void ApplyCheckAll()
        {
            foreach (TourSelectionViewModel tour in Tours)
                tour.IsMarked = CheckAllChecked;
        }

        public void ApplyUncheckAll()
        {
            if (Tours.All(t => t.IsMarked))
                ApplyCheckAll();
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
            if (item is TourViewModel model)
                return model.Name.Contains(FilterText) || (model.From?.Contains(FilterText) ?? false) ||
                    (model.To?.Contains(FilterText) ?? false) || model.TourLogCount.ToString().Contains(FilterText) ||
                    model.SelectedRouteType.ToString().Contains(filterText);


            return false;
        }

        private void UpdateTourView()
        {
            tourViewSource.Source = Tours;
            TourView = tourViewSource.View;

            foreach (TourSelectionViewModel tour in Tours)
                tour.PropertyChanged += TourSelectionViewModelTourChanged;
        }

        private void TourSelectionViewModelTourChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(nameof(TourSelectionViewModel.IsMarked), StringComparison.Ordinal))
            {
                if (Tours.All(t => t.IsMarked))
                    CheckAllChecked = true;
                else if (sender is TourSelectionViewModel current && !current.IsMarked)
                    CheckAllChecked = false;
            }

            TourSelectionViewModelChanged?.Invoke(sender, e);
        }

        private string displayName;
        private bool checkAllChecked;
        private ObservableCollection<TourSelectionViewModel> tours;

        private string filterText;
        private ICollectionView tourView;

        private readonly CollectionViewSource tourViewSource = new CollectionViewSource();
    }
}
