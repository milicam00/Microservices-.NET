using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Users.ReverseAddOwner
{
    public class ReverseAddOwnerCommand : CommandBase<Result>
    {
        public string Username { get; set; }
        public ReverseAddOwnerCommand(string username)
        {
            Username = username;
        }
    }
}
