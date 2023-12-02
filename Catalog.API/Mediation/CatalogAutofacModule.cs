using Autofac;

namespace Catalog.API.Mediation
{
    public class CatalogAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CatalogModule>()
                .As<ICatalogModule>()
                .InstancePerLifetimeScope();
        }
    }
}
