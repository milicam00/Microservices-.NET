using Autofac;
using BuildingBlocks.Application;
using BuildingBlocks.Infrastructure.EventBus;
using MediatR;
using Serilog.Extensions.Logging;
using System.Reflection;
using UserAccess.API.DataAccess;
using UserAccess.API.Mediation;

namespace UserAccess.API.Processing
{
    public class UserAccessStartup
    {
        private static IContainer _container;

        public static void Initialize(
            //IContainer container,
            string connectionString,
            IExecutionContextAccessor executionContextAccessor,
            Serilog.ILogger logger,
            IEventsBus eventsBus
           )
        {

            ConfigureContainer(connectionString,logger, executionContextAccessor, eventsBus);

        }

        private static void ConfigureContainer(
            string connectionString,
             
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
           // containerBuilder.RegisterModule(new EventsBusModule(eventsBus));
            containerBuilder.RegisterModule(new MediatorModule());
            containerBuilder.RegisterModule(new DataAccessModule(connectionString));
            containerBuilder.RegisterInstance(executionContextAccessor);



            _container = containerBuilder.Build();
            UserAccessCompositionRoot.SetContainer(_container);

        }
    }
}
