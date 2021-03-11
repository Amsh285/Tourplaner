using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using Tourplaner.Infrastructure;

namespace Tourplaner
{
    public class ShellViewModel : PropertyChangedBase
    {
        public ObservableCollection<IScreen> Screens
        {
            get
            {
                return screens;
            }
            set
            {
                if (screens != value)
                {
                    screens = value;
                    NotifyPropertyChanged(nameof(Screens));
                }
            }
        }

        public ShellViewModel(IEnumerable<IScreen> screens)
        {
            Assert.NotNull(screens, nameof(screens));

            Screens = new ObservableCollection<IScreen>(screens);
        }

        private ObservableCollection<IScreen> screens;
    }
}
