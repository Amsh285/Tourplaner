﻿using Autofac;
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

        public ShellViewModel(Func<EditTourViewModel> editTourViewModel, Func<HomeViewModel> homeViewModel, Func<TourScreenViewModel> tourScreenViewModel)
        {
            Assert.NotNull(editTourViewModel, nameof(editTourViewModel));
            Assert.NotNull(homeViewModel, nameof(homeViewModel));
            Assert.NotNull(tourScreenViewModel, nameof(tourScreenViewModel));

            this.editTourViewModel = editTourViewModel;
            this.homeViewModel = homeViewModel;
            this.tourScreenViewModel = tourScreenViewModel;
        }

        public void OnMenuSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is ListViewItem item)
            {
                if (item.Name == "ItemHome")
                {
                    SelectedScreen = homeViewModel();
                }
                else if(item.Name == "ItemCreate")
                {
                    SelectedScreen = editTourViewModel();
                }
                else if(item.Name == "ItemOverview")
                {
                    SelectedScreen = tourScreenViewModel();
                }
            }
        }

        private IScreen selectedScreen;
        private readonly Func<EditTourViewModel> editTourViewModel;
        private readonly Func<HomeViewModel> homeViewModel;
        private readonly Func<TourScreenViewModel> tourScreenViewModel;
    }
}
