using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserAccess.Domain.RefreshTokens;

namespace UserAccess.Infrastructure.Domain.RefreshTokens
{
    public class RefreshTokenEntityTypeConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(x => x.TokenId);

            builder.HasOne(b => b.User)
                .WithMany(l => l.RefreshTokens)
                .HasForeignKey(B => B.UserId);

        }
    }
}
