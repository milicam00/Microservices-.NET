using Catalog.Domain.OwnerRentalBooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Domain.OwnerRentalBooks
{
    public class OwnerRentalBookEntityTypeConfiguration : IEntityTypeConfiguration<OwnerRentalBook>
    {
        public void Configure(EntityTypeBuilder<OwnerRentalBook> builder)
        {
            builder.HasKey(x => x.OwnerRentalBookId);

            builder.HasOne(rb => rb.Book)
                .WithMany(b => b.OwnerRentalBooks)
                .HasForeignKey(rb => rb.BookId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(rb => rb.OwnerRental)
                .WithMany(r => r.OwnerRentalBooks)
                .HasForeignKey(rb => rb.OwnerRentalId)
               .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
