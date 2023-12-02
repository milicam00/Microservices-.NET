using Catalog.Domain.OwnerRentals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Domain.OwnerRentals
{
    public class OwnerRentalEntityTypeConfiguration : IEntityTypeConfiguration<OwnerRental>
    {
        public void Configure(EntityTypeBuilder<OwnerRental> builder)
        {
            builder.HasKey(x => x.OwnerRentalId);

            builder.HasOne(o => o.Owner)
                .WithMany()
                .HasForeignKey(O => O.OwnerId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
