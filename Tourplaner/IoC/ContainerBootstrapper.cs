using Autofac;
using System;
using System.Reflection;
using System.Linq;
using Tourplaner.Infrastructure.Configuration;
using Tourplaner.Infrastructure;
using Tourplaner.Infrastructure.Logging;
using Tourplaner.Infrastructure.Database;
using Tourplaner.UI;
using Tourplaner.ViewModels;

namespace Tourplaner.IoC
{
    public sealed class ContainerBootstrapper
    {
        public IContainer Build(TourplanerConfig config)
        {
            Assert.NotNull(config, nameof(config));
            Assert.NotNull(config.DbSettings, nameof(config.DbSettings));

            ContainerBuilder builder = new ContainerBuilder();
            Assembly assembly = Assembly.GetExecutingAssembly();

            RegisterConfig(builder, config);
            RegisterViewModels(builder, assembly);
            RegisterLogger(builder);
            RegisterDatabase(builder);
            RegisterRepositories(builder, assembly);
            RegisterEntities(builder, assembly);
            RegisterRequests(builder, assembly);
            RegisterInfraStructure(builder);

            return builder.Build();
        }

        private static void RegisterConfig(ContainerBuilder builder, TourplanerConfig config)
        {
            builder.RegisterInstance(config)
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterInstance(config.DbSettings)
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterInstance(config.RouteImageSettings)
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();
        }

        private static void RegisterViewModels(ContainerBuilder builder, Assembly assembly)
        {
            Type[] viewModels = assembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("ViewModel"))
                .ToArray();

            builder.RegisterTypes(viewModels)
                .AsSelf()
                .AsImplementedInterfaces();
        }

        private void RegisterLogger(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(NLogLogger<>))
                .AsImplementedInterfaces()
                .AsSelf();
        }

        private void RegisterDatabase(ContainerBuilder builder)
        {
            builder.RegisterType<PostgreSqlDatabase>()
                .AsSelf()
                .SingleInstance();
        }

        private void RegisterRepositories(ContainerBuilder builder, Assembly assembly)
        {
            Type[] repositories = assembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Repository"))
                .ToArray();

            builder.RegisterTypes(repositories)
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();
        }

        private void RegisterEntities(ContainerBuilder builder, Assembly assembly)
        {
            Type[] entities = assembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Entity"))
                .ToArray();

            builder.RegisterTypes(entities)
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();
        }

        private void RegisterRequests(ContainerBuilder builder, Assembly assembly)
        {
            Type[] requests = assembly
                .GetTypes()
                .Where(t => t.Name.EndsWith("Request"))
                .ToArray();

            builder.RegisterTypes(requests)
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();
        }

        private void RegisterInfraStructure(ContainerBuilder builder)
        {
            builder.RegisterType<MessageBoxService>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
