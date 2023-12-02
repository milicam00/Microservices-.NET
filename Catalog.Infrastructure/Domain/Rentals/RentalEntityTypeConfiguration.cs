using Catalog.Domain.Rentals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Domain.Rentals
{
    public class RentalEntityTypeConfiguration : IEntityTypeConfiguration<Rental>
    {
        public void Configure(EntityTypeBuilder<Rental> builder)
        {
            builder.HasKey(x => x.RentalId);


            builder.HasOne(r => r.Reader)
                .WithMany()
                .HasForeignKey(r => r.ReaderId);
        }
    }
}
