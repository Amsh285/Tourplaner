using Autofac;
using NUnit.Framework;
using System;
using System.Windows;
using Tourplaner.Infrastructure.Configuration;
using Tourplaner.IoC;
using Tourplaner.UI;
using Assert = NUnit.Framework.Assert;

namespace Tourplaner.Tests
{
    public sealed class ShellViewDataTemplateSelectorTests
    {
        [SetUp]
        public void Setup()
        {
            dataTemplateselector = new ShellViewDataTemplateSelector();

            TourplanerConfig config = new TourplanerConfig()
            {
                DbSettings = new DatabaseSettings(),
                RouteImageSettings = new RouteImageStorageSettings()
            };

            ContainerBootstrapper bootstrapper = new ContainerBootstrapper();
            container = bootstrapper.Build(config);
        }

        [Test]
        public void SelectTemplateAssignsViewByNamingConventions()
        {
            TourScreenViewModel screenViewModel = container.Resolve<TourScreenViewModel>();
            Type expectedViewType = typeof(TourScreenView);

            DataTemplate template = dataTemplateselector.SelectTemplate(screenViewModel, null);
            Assert.NotNull(template);
            Assert.NotNull(template.VisualTree);
            Assert.AreEqual(expectedViewType, template.VisualTree.Type);
        }

        [Test]
        public void SelectTemplateViewNotFoundThrows()
        {
            FooScreenViewModel screenViewModel = new FooScreenViewModel();
            Assert.Throws<ViewNotFoundException>(() => dataTemplateselector.SelectTemplate(screenViewModel, null));
        }

        private ShellViewDataTemplateSelector dataTemplateselector;
        private IContainer container;
    }
}
