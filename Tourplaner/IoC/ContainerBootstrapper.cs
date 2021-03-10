using Autofac;
using System;
using System.Reflection;
using System.Linq;
using Tourplaner.Infrastructure.Configuration;
using Tourplaner.Infrastructure;

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
    }
}
