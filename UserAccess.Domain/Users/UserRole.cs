using BuildingBlocks.Domain;
using System.ComponentModel.DataAnnotations;

namespace UserAccess.Domain.Users
{
    public class UserRole : ValueObject
    {
        [Key]
        public Guid Id { get; set; }
        public static UserRole Reader => new UserRole(nameof(Reader));

        public static UserRole Administrator => new UserRole(nameof(Administrator));

        public static UserRole Owner => new UserRole(nameof(Owner));

        public Guid UserId { get; set; }
        public string Value { get; }

        public UserRole()
        {
            Id = Guid.NewGuid();
        }
        public UserRole(string value)
        {
            Id = Guid.NewGuid();
            this.Value = value;
        }
    }
}
