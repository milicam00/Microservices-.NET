using Catalog.Domain.Books;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Domain.Books
{
    public class BookEntityTypeConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(x => x.BookId);

            builder.Property(b => b.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.Author)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(b => b.Pages)
               .IsRequired()
               .HasDefaultValue(0);

            builder.HasOne(b => b.Library)
            .WithMany(l => l.Books)
            .HasForeignKey(b => b.LibraryId);

            builder.HasOne(b => b.Image)
                 .WithOne()
                 .HasForeignKey<Book>(b => b.ImageId)
                 .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
