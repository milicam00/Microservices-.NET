using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Users.ChangeReaderPersonalInfo
{
    public class ChangeReaderPersonalInfoCommand : CommandBase<Result>
    {
        public ChangeReaderPersonalInfoCommand(string oldUsername, string? newUsername, string? newFirstName, string? newLastName)
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
