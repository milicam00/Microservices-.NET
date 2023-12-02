using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Users.ChangeOwnerPersonalInfo
{
    public class ChangeOwnerPersonalInfoCommand : CommandBase<Result>
    {
        public ChangeOwnerPersonalInfoCommand(string oldUsername, string? newUsername, string? newFirstName, string? newLastName)
        {
            OldUsername = oldUsername;
            NewUsername = newUsername;
            NewFirstName = newFirstName;
            NewLastName = newLastName;
        }
        public string OldUsername { get; set; }
        public string? NewUsername { get; set; }
        public string? NewFirstName { get; set; }
        public string? NewLastName { get; set; }
    }
}
