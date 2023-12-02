using UserAccess.Domain.Users;
using UserAccess.Infrastructure.Contracts;

namespace UserAccess.API.Application.ChangePersonalInfo
{
    public class ChangePersonalInfoCommand : CommandBase<List<UserRole>>
    {
        public ChangePersonalInfoCommand(string userName, string? newUsername, string? newFirstName, string? newLastName)
        {
            UserName = userName;
            NewUsername = newUsername;
            NewFirstName = newFirstName;
            NewLastName = newLastName;
        }
        public string UserName { get; set; }
        public string? NewUsername { get; set; }
        public string? NewFirstName { get; set; }
        public string? NewLastName { get; set; }

    }
}
