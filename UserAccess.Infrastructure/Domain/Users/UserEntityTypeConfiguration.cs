using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserAccess.Domain.Users;

namespace UserAccess.Infrastructure.Domain.Users
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {

            builder.HasKey(x => x.UserId);

            builder.OwnsMany<UserRole>("Roles", b =>
            {
                b.WithOwner().HasForeignKey("UserId");
                b.ToTable("UserRoles", "users");
                b.Property<Guid>("UserId");
                b.Property<string>("Value").HasColumnName("RoleCode");
                b.HasKey("UserId");
            });

            builder.HasMany(t => t.RefreshTokens)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId)
                .IsRequired();


            builder.Property(n => n.UserName)
                .HasMaxLength(75)
                .IsRequired();

            builder.Property(n => n.FirstName)
                .HasMaxLength(75)
                .IsRequired();

            builder.Property(n => n.LastName)
                .HasMaxLength(75)
                .IsRequired();

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.HasIndex(p => p.UserName)
                .IsUnique();

            builder.HasOne(i => i.ProfileImage)
                .WithOne()
                .HasForeignKey<User>(b => b.ProfileImageId);
        }
    }
}
