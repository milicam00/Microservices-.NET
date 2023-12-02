using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Users.ReverseAddReader
{
    public class ReverseAddReaderCommand : CommandBase<Result>
    {
        public string Username { get; set; }
        public ReverseAddReaderCommand(string username)
        {
            Username = username;
        }
    }
}
