using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Users.AddOwner
{
    public class AddOwnerCommand : CommandBase<Result>
    {
        public AddOwnerCommand(
            string username,
            string email,
            string firstName,
            string lastName
            )
        {
            UserName = username;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
