using Autofac;
using NLog;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using Tourplaner.Infrastructure;
using Tourplaner.Infrastructure.Configuration;
using Tourplaner.Infrastructure.Logging;
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
            ILogger<App> logger = new NLogLogger<App>();

            try
            {
                TourplanerConfigReader configReader = new TourplanerConfigReader();
                TourplanerConfig config = configReader.GetTourplanerConfig();

                ContainerBootstrapper bootstrapper = new ContainerBootstrapper();
                IContainer container = bootstrapper.Build(config);

                ShellView shellView = new ShellView();
                ShellViewModel shellViewModel = container.Resolve<ShellViewModel>();
                shellView.DataContext = shellViewModel;

                shellView.Show();
            }
            catch(TourplanerConfigReaderException configReaderEx)
            {
                logger.Error($"Fehler beim laden der Konfiguration: {configReaderEx.Message}");
                this.Shutdown();
            }
            catch (Exception ex)
            {
                logger.Error($"Unexpected Error: {ex.Message}");
                this.Shutdown();
            }
        }
    }
}
