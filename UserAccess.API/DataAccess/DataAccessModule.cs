using Autofac;
using BuildingBlocks.Application.Data;
using BuildingBlocks.Infrastructure;
using Microsoft.EntityFrameworkCore;
using UserAccess.Domain.APIKeys;
using UserAccess.Domain.ProfileImages;
using UserAccess.Domain.RefreshTokens;
using UserAccess.Domain.Users;
using UserAccess.Infrastructure;
using UserAccess.Infrastructure.Domain.APIKeys;
using UserAccess.Infrastructure.Domain.ProfileImages;
using UserAccess.Infrastructure.Domain.RefreshTokens;
using UserAccess.Infrastructure.Domain.Users;

namespace UserAccess.API.DataAccess
{
    public class DataAccessModule : Autofac.Module
    {
        private readonly string _databaseConnectionString;
        //private readonly ILoggerFactory _loggerFactory;

        public DataAccessModule(string databaseConnectionString)
        {
            _databaseConnectionString = databaseConnectionString;
            //_loggerFactory = loggerFactory;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SqlConnectionFactory>()
               .As<ISqlConnectionFactory>()
               .WithParameter("connectionString", _databaseConnectionString)
               .InstancePerLifetimeScope();

            builder
               .Register(c =>
               {
                   var dbContextOptionsBuilder = new DbContextOptionsBuilder<UserAccessContext>();
                   dbContextOptionsBuilder.UseSqlServer(_databaseConnectionString);

                   return new UserAccessContext(dbContextOptionsBuilder.Options /*_loggerFactory*/);
               })
               .AsSelf()
               .As<DbContext>()
               .InstancePerLifetimeScope();

            builder.RegisterType<UserRepository>()
               .As<IUserRepository>()
               .InstancePerLifetimeScope();

            builder.RegisterType<RefreshTokenRepository>()
                 .As<IRefreshTokenRepository>()
                 .InstancePerLifetimeScope();

            builder.RegisterType<APIKeyRepository>()
                .As<IAPIKeyRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ProfileImageRepository>()
                .As<IProfileImageRepository>()
                .InstancePerLifetimeScope();
        }
    }
}
