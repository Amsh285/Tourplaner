using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Tourplaner
{
    /// <summary>
    /// Woohoo...
    /// </summary>
    public class SavePersonCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public SavePersonCommand(MainWindowViewModel mainWindow)
        {
            this.mainWindow = mainWindow ?? throw new ArgumentNullException(nameof(mainWindow));
            this.user = mainWindow.User;
        }

        public bool CanExecute(object parameter)
        {
            return !string.IsNullOrWhiteSpace(user.Firstname) && !string.IsNullOrWhiteSpace(user.Lastname);
        }

        public void Execute(object parameter)
        {
            mainWindow.DisplayText = $"Hello {user.Firstname} {user.Lastname}";
        }

        public void NotifyCanExecuteChanged(object sender, EventArgs args)
        {
            CanExecuteChanged(sender, args);
        }

        private readonly MainWindowViewModel mainWindow;
        private readonly Person user;
    }
}
