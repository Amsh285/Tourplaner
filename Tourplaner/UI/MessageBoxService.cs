using System;
using System.Windows;
using Tourplaner.Infrastructure;

namespace Tourplaner.UI
{
    public sealed class MessageBoxService
    {
        public void ShowInfo(string message, string caption = "Infomation")
        {
            Assert.NotNull(message, nameof(message));

            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowError(Exception error, string caption = "Error")
        {
            Assert.NotNull(error, nameof(error));

            ShowError(error.Message, caption);
        }

        public void ShowError(string message, string caption = "Error")
        {
            Assert.NotNull(message, nameof(message));

            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
