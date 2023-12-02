using BuildingBlocks.Domain;
using UserAccess.Infrastructure.Contracts;

namespace UserAccess.API.Application.RegisterOwner
{
    public class RegisterOwnerCommand : CommandBase<Result>
    {
        public RegisterOwnerCommand(
             string username,
            string password,
             string email,
            string firstName,
            string lastName
            )
        {
            Username = username;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Email = email;

        }
        public string Username { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string Email { get; }

        public string Password { get; }
    }
}
