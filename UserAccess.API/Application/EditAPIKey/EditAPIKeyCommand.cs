using BuildingBlocks.Domain;
using UserAccess.Infrastructure.Contracts;

namespace UserAccess.API.Application.EditAPIKey
{
    public class EditAPIKeyCommand : CommandBase<Result>
    {
        public EditAPIKeyCommand(string username, string newName)
        {
            Username = username;
            NewName = newName;
        }
        public string Username { get; set; }
        public string NewName { get; set; }
    }
}
