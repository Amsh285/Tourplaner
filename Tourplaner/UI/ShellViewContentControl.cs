using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Tourplaner.UI
{
    public sealed class ShellViewContentControl : ContentControl
    {
        public ShellViewContentControl()
        {
            DataContextChanged += ShellViewContentControl_DataContextChanged;
        }

        private void ShellViewContentControl_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
