using Autofac;

namespace UserAccess.API.Mediation
{
    public class UserAccessCompositionRoot
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
