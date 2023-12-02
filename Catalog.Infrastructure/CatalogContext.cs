using Catalog.Domain.BookImages;
using Catalog.Domain.Books;
using Catalog.Domain.Libraries;
using Catalog.Domain.OutboxMessages;
using Catalog.Domain.OwnerRentalBooks;
using Catalog.Domain.OwnerRentals;
using Catalog.Domain.Owners;
using Catalog.Domain.Readers;
using Catalog.Domain.RentalBooks;
using Catalog.Domain.Rentals;
using Catalog.Infrastructure.Domain.BookImages;
using Catalog.Infrastructure.Domain.Books;
using Catalog.Infrastructure.Domain.Libraries;
using Catalog.Infrastructure.Domain.OutboxMessages;
using Catalog.Infrastructure.Domain.OwnerRentalBooks;
using Catalog.Infrastructure.Domain.OwnerRentals;
using Catalog.Infrastructure.Domain.Owners;
using Catalog.Infrastructure.Domain.Readers;
using Catalog.Infrastructure.Domain.RentalBooks;
using Catalog.Infrastructure.Domain.Rentals;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Catalog.Infrastructure
{
    public class CatalogContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        public DbSet<RentalBook> RentalBooks { get; set; }
        public DbSet<OwnerRental> OwnerRentals { get; set; }
        public DbSet<OwnerRentalBook> OwnerRentalBooks { get; set; }
        public DbSet<Image> Images { get; set; }


        private readonly ILoggerFactory _loggerFactory;

        public CatalogContext(DbContextOptions<CatalogContext> options, ILoggerFactory loggerFactory)
           : base(options)
        {
            _loggerFactory = loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            string connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CatalogDB;Integrated Security=True;Pooling=False";
            optionsBuilder.UseSqlServer(connection, b => b.MigrationsAssembly("Catalog.API"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BookEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ImageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LibraryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());    
            modelBuilder.ApplyConfiguration(new OwnerRentalBookEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OwnerRentalEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OwnerEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ReaderEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RentalBookEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RentalEntityTypeConfiguration());

        }
    }
}
