using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;
using MiniMapper.Infrastructure.Data.Access;
using NHibernate;

namespace MiniMapper
{
    public static class ContainerConfiguration
    {
        public static IUnityContainer GetConfiguredContainer()
        {
            var container = new UnityContainer();

            container.RegisterInstance(typeof (IStatelessSession), "Session", NHibernateHelper.GetStatelessSession());
            RegisterComponentByInterface(container, "MiniMapper.Infrastructure.Data");
            RegisterComponentByInterface(container, "MiniMapper.Application.Services");
            RegisterComponentByClass(container, "MiniMapper.Application.Forms");

            return container;
        }

        private static void RegisterComponentByClass(IUnityContainer container, string assemblyName)
        {
            var assembly = Assembly.Load(assemblyName);

            var implementations = assembly.DefinedTypes
                                          .Where(dt => dt.IsClass && !dt.GetInterfaces().Any())
                                          .ToList();

            foreach (var implementation in implementations)
                container.RegisterType(implementation, implementation.Name);
        }

        private static void RegisterComponentByInterface(IUnityContainer container, string assemblyName)
        {
            var assembly = Assembly.Load(assemblyName);

            var abstractions = assembly.DefinedTypes
                                       .Where(dt => dt.IsInterface)
                                       .ToList();

            foreach (var abstraction in abstractions)
            {
                var implementations = assembly.DefinedTypes
                                              .Where(dt => dt.IsClass && dt.IsInstanceOfType(abstraction))
                                              .ToList();

                foreach (var implementation in implementations)
                    container.RegisterType(abstraction, implementation, implementation.Name);
            }
        }
    }
}
