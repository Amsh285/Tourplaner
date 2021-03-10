using Autofac;
using System;
using System.Reflection;
using System.Linq;
using Tourplaner.Infrastructure.Configuration;
using Tourplaner.Infrastructure;
using Tourplaner.Infrastructure.Logging;

namespace Tourplaner.IoC
{
    public sealed class ContainerBootstrapper
    {
        public IContainer Build(TourplanerConfig config)
        {
            Assert.NotNull(config, nameof(config));

            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterInstance(config)
                .AsSelf()
                .AsImplementedInterfaces();

            Assembly assembly = Assembly.GetExecutingAssembly();
            RegisterViewModels(builder, assembly);
            RegisterLogger(builder);

            return builder.Build();
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
    }
}
