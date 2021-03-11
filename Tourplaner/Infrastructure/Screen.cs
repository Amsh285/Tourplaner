using System;
using System.Collections.Generic;
using System.Text;

namespace Tourplaner.Infrastructure
{
    public abstract class Screen : PropertyChangedBase, IScreen
    {
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

        public Screen(string displayName)
        {
            Assert.NotNull(displayName, nameof(displayName));

            DisplayName = displayName;
        }

        private string displayName;
    }
}
