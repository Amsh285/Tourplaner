using Autofac;
using Autofac.Core.Registration;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Tourplaner.Infrastructure.Configuration;
using Tourplaner.Infrastructure.Database;
using Tourplaner.Infrastructure.Logging;
using Tourplaner.IoC;
using Tourplaner.UI;

namespace Tourplaner.Tests
{
    public sealed class ContainerBootstrapperTests
    {
        [SetUp]
        public void Setup()
        {
            TourplanerConfig config = new TourplanerConfig()
            {
                DbSettings = new DatabaseSettings(),
                RouteImageSettings = new RouteImageStorageSettings()
            };

            ContainerBootstrapper bootstrapper = new ContainerBootstrapper();
            container = bootstrapper.Build(config);

            executingAssembly = Assembly.GetExecutingAssembly();
        }

        [Test]
        public void RegistersConfig()
        {
            AssertComponentRegistered<TourplanerConfig>();
            AssertComponentRegistered<DatabaseSettings>();
            AssertComponentRegistered<RouteImageStorageSettings>();
        }

        [Test]
        public void RegistersViewModels()
        {
            Type[] viewModels = executingAssembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("ViewModel") && !t.Name.Equals(typeof(FooScreenViewModel).Name))
                .ToArray();

            foreach (Type viewModelType in viewModels)
                AssertComponentRegistered(viewModelType);
        }

        [Test]
        public void RegisteresGenericLogger()
        {
            AssertComponentRegistered<ILogger<int>>();
            AssertComponentRegistered<ILogger<string>>();
            AssertComponentRegistered<ILogger<object>>();
        }

        [Test]
        public void RegistersDatabase()
        {
            AssertComponentRegistered<PostgreSqlDatabase>();
        }

        [Test]
        public void RegistersRepositories()
        {
            Type[] repositories = executingAssembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Repository"))
                .ToArray();

            foreach (Type repositoryType in repositories)
                AssertComponentRegistered(repositoryType);
        }

        [Test]
        public void RegistersEntities()
        {
            Type[] entities = executingAssembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Entity"))
                .ToArray();

            foreach (Type entityType in entities)
                AssertComponentRegistered(entityType);
        }

        [Test]
        public void RegistersRequests()
        {
            Type[] requests = executingAssembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Request"))
                .ToArray();

            foreach (Type requestType in requests)
                AssertComponentRegistered(requestType);
        }

        [Test]
        public void RegistersInfrastructure()
        {
            AssertComponentRegistered<MessageBoxService>();
        }

        private void AssertComponentRegistered<TComponent>() => AssertComponentRegistered(typeof(TComponent));

        public void AssertComponentRegistered(Type componentType)
        {
            Assert.DoesNotThrow(
                () => container.Resolve(componentType),
                $"Error Resolving {componentType}"
            );
        }

        private IContainer container;
        private Assembly executingAssembly;
    }
}
