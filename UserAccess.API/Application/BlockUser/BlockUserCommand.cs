using BuildingBlocks.Domain;
using UserAccess.Infrastructure.Contracts;

namespace UserAccess.API.Application.BlockUser
{
    public class BlockUserCommand : CommandBase<Result>
    {
        public BlockUserCommand(string username)
        {
            Username = username;
        }

        public string Username { get; set; }
    }
}
