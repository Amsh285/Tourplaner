using Autofac;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Tourplaner.IoC;

namespace Tourplaner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ContainerBootstrapper bootstrapper = new ContainerBootstrapper();
            IContainer container = bootstrapper.Build();

            MainWindowView mainWindow = new MainWindowView();
            mainWindow.Show();
        }
    }
}
