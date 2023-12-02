using BuildingBlocks.Domain;
using UserAccess.Infrastructure.Contracts;

namespace UserAccess.API.Application.DeleteAPIKey
{
    public class DeleteApiKeyCommand : CommandBase<Result>
    {
        public DeleteApiKeyCommand(string username)
        {
            Username = username;
        }

        public string Username { get; set; }
    }
}
