using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Users.UnblockOwner
{
    public class UnblockOwnerCommand : CommandBase<Result>
    {
        public UnblockOwnerCommand(string userName)
        {
            UserName = userName;
        }
        public string UserName { get; set; }
    }
}
