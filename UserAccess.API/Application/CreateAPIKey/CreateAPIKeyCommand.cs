using BuildingBlocks.Domain;
using UserAccess.Infrastructure.Contracts;

namespace UserAccess.API.Application.CreateAPIKey
{
    public class CreateAPIKeyCommand : CommandBase<Result>
    {
        public CreateAPIKeyCommand(string username, string name)
        {
            Username = username;
            KeyName = name;
        }
        public string Username { get; set; }
        public string KeyName { get; set; }
    }
}
