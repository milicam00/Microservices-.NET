using UserAccess.Domain.Users;
using UserAccess.Infrastructure.Domain.Users;

namespace UserAccess.Infrastructure.Domain
{
    public class AdminSeedData
    {
        private UserAccessContext _context;

        public AdminSeedData(UserAccessContext context)
        {
            _context = context;
        }

        public async void SeedAdminUser()
        {
            var password = PasswordManager.HashPassword("admin");
            var admin = new User
            {
                UserName = "admin",
                Password = password,
                Email = "admin@gmail.com",
                IsActive = true,
                FirstName = "Admin",
                LastName = "User",
                Roles = new List<UserRole> { UserRole.Administrator }
            };

            if (!_context.Users.Any(u => u.UserName == admin.UserName))
            {
                _context.Users.Add(admin);
                _context.SaveChanges();
            }

        }
    }
}
