using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Users.UnblockReader
{
    public class UnblockReaderCommand : CommandBase<Result>
    {
        public UnblockReaderCommand(string userName)
        {
            UserName = userName;
        }
        public string UserName { get; set; }
    }
}
