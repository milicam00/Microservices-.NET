using Autofac;
using BuildingBlocks.Application;
using BuildingBlocks.Infrastructure.EventBus;
using Catalog.API.DataAccess;
using Catalog.API.Mediation;
using MediatR;
using Serilog.Extensions.Logging;
using System.Reflection;

namespace Catalog.API.Processing
{
    public class CatalogStartup
    {
        private static IContainer _container;

        public static void Initialize(
           string connectionString, string smtpServer, int smtpPort, bool enableSsl, string smtpUsername, string smtpPassword,
            IExecutionContextAccessor executionContextAccessor,
            Serilog.ILogger logger,
           IEventsBus eventsBus

           )
        {

            ConfigureContainer(connectionString, smtpServer, smtpPort, enableSsl, smtpUsername, smtpPassword, logger, executionContextAccessor, eventsBus);
        }

        private static void ConfigureContainer(
            string connectionString,
            string smtpServer,
            int smtpPort,
            bool enableSsl,
            string smtpUsername,
            string smtpPassword,
            Serilog.ILogger logger,

            IExecutionContextAccessor executionContextAccessor,
            IEventsBus eventsBus)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            var loggerFactory = new SerilogLoggerFactory(logger);

            containerBuilder.RegisterModule(new ProcessingModule());
          //  containerBuilder.RegisterModule(new EventsBusModule(eventsBus));
            containerBuilder.RegisterModule(new MediatorModule());
            containerBuilder.RegisterModule(new DataAccessModule(connectionString, smtpServer, smtpPort, enableSsl, smtpUsername, smtpPassword, loggerFactory));

           
            containerBuilder.RegisterInstance(executionContextAccessor);

            _container = containerBuilder.Build();
            CatalogCompositionRoot.SetContainer(_container);

        }
    }
}
