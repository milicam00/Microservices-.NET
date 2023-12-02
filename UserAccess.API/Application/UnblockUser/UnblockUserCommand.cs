using BuildingBlocks.Domain;
using UserAccess.Infrastructure.Contracts;

namespace UserAccess.API.Application.UnblockUser
{
    public class UnblockUserCommand : CommandBase<Result>
    {
        public UnblockUserCommand(string username)
        {
            Username = username;
        }
        public string Username { get; set; }
    }
}
