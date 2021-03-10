using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Tourplaner.Infrastructure
{
    interface IHasView
    {
        void Loaded(object sender, RoutedEventArgs e);
    }
}
