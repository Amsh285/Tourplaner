using Autofac;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Tourplaner.Infrastructure;
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

            //MainWindowViewModel viewModel = container.Resolve<MainWindowViewModel>();

            //MainWindowView mainWindow = new MainWindowView();
            //mainWindow.DataContext = viewModel;

            //ViewModelBinder binder = new ViewModelBinder();
            //binder.Bind(viewModel, mainWindow);

            //mainWindow.Show();

            ViewModelBinder binder = new ViewModelBinder();

            ShellView shellView = new ShellView();
            ShellViewModel shellViewModel = container.Resolve<ShellViewModel>();
            shellView.DataContext = shellViewModel;

            binder.Bind(shellViewModel, shellView);

            shellViewModel.Tabs.CollectionChanged += Tabs_CollectionChanged;

            shellView.Show();
        }

        private void Tabs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        //private IDictionary<PropertyChangedBase, FrameworkElement> 
    }
}
