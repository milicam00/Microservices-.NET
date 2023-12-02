using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserAccess.Domain.APIKeys;
using UserAccess.Domain.ProfileImages;
using UserAccess.Domain.RefreshTokens;
using UserAccess.Domain.Users;
using UserAccess.Infrastructure.Domain.APIKeys;
using UserAccess.Infrastructure.Domain.ProfileImages;
using UserAccess.Infrastructure.Domain.RefreshTokens;
using UserAccess.Infrastructure.Domain.Users;

namespace UserAccess.Infrastructure
{
    public class UserAccessContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<APIKey> APIKeys { get; set; }
        public DbSet<ProfileImage> ProfileImages { get; set; }

        private readonly ILoggerFactory _loggerFactory;

        public UserAccessContext(DbContextOptions options /*ILoggerFactory loggerFactory*/)
            : base(options)
        {
           // _loggerFactory = loggerFactory;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new APIKeyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProfileImageEntityTypeConfiguration());
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=UserAccessDB;Integrated Security=True;Pooling=False";
            optionsBuilder.UseSqlServer(connection, b => b.MigrationsAssembly("UserAccess.API"));
        }
    }
}
