using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Users.BlockReader
{
    public class BlockReaderCommand : CommandBase<Result>
    {
        public BlockReaderCommand(string userName)
        {
            UserName = userName;
        }
        public string UserName { get; set; }
    }
}
