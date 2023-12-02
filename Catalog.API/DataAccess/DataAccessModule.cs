using Autofac;
using BuildingBlocks.Application.Data;
using BuildingBlocks.Application.Emails;
using BuildingBlocks.Application.ICsvGeneration;
using BuildingBlocks.Application.IXlsxGeneration;
using BuildingBlocks.Application.XmlGeneration;
using BuildingBlocks.Infrastructure;
using Catalog.Domain.BookImages;
using Catalog.Domain.Books;
using Catalog.Domain.Libraries;
using Catalog.Domain.OutboxMessages;
using Catalog.Domain.OwnerRentals;
using Catalog.Domain.Owners;
using Catalog.Domain.Readers;
using Catalog.Domain.Rentals;
using Catalog.Infrastructure;
using Catalog.Infrastructure.Domain.BookImages;
using Catalog.Infrastructure.Domain.Books;
using Catalog.Infrastructure.Domain.Libraries;
using Catalog.Infrastructure.Domain.OutboxMessages;
using Catalog.Infrastructure.Domain.OwnerRentals;
using Catalog.Infrastructure.Domain.Owners;
using Catalog.Infrastructure.Domain.Readers;
using Catalog.Infrastructure.Domain.Rentals;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.DataAccess
{
    public class DataAccessModule : Autofac.Module
    {
        private readonly string _databaseConnectionString;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly bool _enableSsl;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;


        private readonly ILoggerFactory _loggerFactory;
        public DataAccessModule(string databaseConnectionString, string smtpServer, int smtpPort, bool enableSsl, string smtpUsername, string smtpPassword, ILoggerFactory loggerFactory)
        {
            _databaseConnectionString = databaseConnectionString;
            _loggerFactory = loggerFactory;
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _enableSsl = enableSsl;
            _smtpUsername = smtpUsername;
            _smtpPassword = smtpPassword;


        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SqlConnectionFactory>()
                .As<ISqlConnectionFactory>()
                .WithParameter("connectionString", _databaseConnectionString)
                .InstancePerLifetimeScope();
            builder.RegisterType<CsvGenerationService>().As<ICsvGenerationService>();
            builder.RegisterType<XlsxGenerationService>().As<IXlsxGenerationService>();
            builder.RegisterType<XmlGenerationService>().As<IXmlGenerationService>();
           // builder.RegisterType<UploadImageService>().As<IUploadImageService>();

            builder.RegisterType<SmtpEmailService>()
                .As<IEmailService>()
                .WithParameter("smtpServer", _smtpServer)
                .WithParameter("smtpPort", _smtpPort)
                .WithParameter("enableSsl", _enableSsl)
                .WithParameter("smtpUsername", _smtpUsername)
                .WithParameter("smtpPassword", _smtpPassword)

                .InstancePerLifetimeScope();

            builder
                .Register(c =>
                {
                    var dbContextOptionsBuilder = new DbContextOptionsBuilder<CatalogContext>();
                    dbContextOptionsBuilder.UseSqlServer(_databaseConnectionString);

                    return new CatalogContext(dbContextOptionsBuilder.Options, _loggerFactory);
                })
                .AsSelf()
                .As<DbContext>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BookRepository>()
                .As<IBookRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<LibraryRepository>()
                .As<ILibraryRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RentalRepository>()
                .As<IRentalRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OutboxMessageRepository>()
                .As<IOutboxMessageRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ReaderRepository>()
                .As<IReaderRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OwnerRepository>()
                .As<IOwnerRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OwnerRentalRepository>()
                .As<IOwnerRentalRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ImageRepository>()
                .As<IImageRepository>()
                .InstancePerLifetimeScope();
        }
    }
}
