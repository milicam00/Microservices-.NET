using Autofac;

namespace Catalog.API.Mediation
{
    internal static class CatalogCompositionRoot
    {
        private static IContainer? _container;

        public static void SetContainer(Autofac.IContainer container)
        {
            _container = container;
        }

        internal static ILifetimeScope BeginLifetimeScope()
        {
            return _container.BeginLifetimeScope();
        }
    }
}
