using Catalog.Domain.Libraries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Domain.Libraries
{
    public class LibraryEntityTypeConfiguration : IEntityTypeConfiguration<Library>
    {
        public void Configure(EntityTypeBuilder<Library> builder)
        {
            builder.HasKey(x => x.LibraryId);

            builder.Property(n => n.LibraryName)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasMany(e => e.Books)
                .WithOne(e => e.Library)
                .HasForeignKey(e => e.LibraryId)
                .IsRequired();

            builder.HasOne(b => b.Owner)
                .WithMany(l => l.Libraries)
                .HasForeignKey(b => b.OwnerId);
        }
    }
}
