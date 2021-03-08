using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Tourplaner.Infrastructure;

namespace Tourplaner
{
    public class MainWindowViewModel : PropertyChangedBase
    {
        public Person User { get; }

        public string Firstname
        {
            get { return User.Firstname; }
            set
            {
                if (User.Firstname != value)
                {
                    User.Firstname = value;
                    NotifyPropertyChanged(nameof(Firstname));
                    NotifyPropertyChanged(nameof(DisplayFirstNameText));
                }
            }
        }

        public string Lastname
        {
            get { return User.Lastname; }
            set
            {
                if (User.Lastname != value)
                {
                    User.Lastname = value;
                    NotifyPropertyChanged(nameof(Lastname));
                    NotifyPropertyChanged(nameof(DisplayLastNameText));
                }
            }
        }

        public string DisplayFirstNameText { get { return Firstname; } }

        public string DisplayLastNameText { get { return Lastname; } }

        public string DisplayText
        {
            get { return displayText; }
            set
            {
                if (displayText != value)
                {
                    displayText = value;
                    NotifyPropertyChanged(nameof(DisplayText));
                }
            }
        }

        public ICommand SavePersonCommand => savePersonCommand;

        public MainWindowViewModel()
        {
            this.User = new Person();
            this.savePersonCommand = new SavePersonCommand(this);
        }

        public override void NotifyPropertyChanged(string name)
        {
            base.NotifyPropertyChanged(name);
            savePersonCommand.NotifyCanExecuteChanged(this, EventArgs.Empty);
        }

        private string displayText;
        private readonly SavePersonCommand savePersonCommand;
    }
}
