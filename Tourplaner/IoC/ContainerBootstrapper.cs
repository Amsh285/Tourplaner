using Autofac;
using System;
using System.Reflection;
using System.Linq;

namespace Tourplaner.IoC
{
    public sealed class ContainerBootstrapper
    {
        public IContainer Build()
        {
            ContainerBuilder builder = new ContainerBuilder();

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
