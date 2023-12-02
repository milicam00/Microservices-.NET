using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserAccess.Domain.ProfileImages;

namespace UserAccess.Infrastructure.Domain.ProfileImages
{
    public class ProfileImageEntityTypeConfiguration : IEntityTypeConfiguration<ProfileImage>
    {
        public void Configure(EntityTypeBuilder<ProfileImage> builder)
        {
            builder.HasKey(x => x.ProfileImageId);

            builder.Property(x => x.Dimensions)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Format)
                .HasMaxLength(100)
                .IsRequired();

        }
    }
}
