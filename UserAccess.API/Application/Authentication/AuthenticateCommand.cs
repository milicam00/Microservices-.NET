using BuildingBlocks.Domain;
using UserAccess.Infrastructure.Contracts;

namespace UserAccess.API.Application.Authentication
{
    public class AuthenticateCommand : CommandBase<Result>
    {
        public AuthenticateCommand(string username, string password)
        {
            UserName = username;
            Password = password;
        }
        public string UserName { get; }
        public string Password { get; }
    }
}
