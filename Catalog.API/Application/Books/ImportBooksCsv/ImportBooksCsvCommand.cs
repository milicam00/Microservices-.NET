using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Books.ImportBooksCsv
{
    public class ImportBooksCsvCommand : CommandBase<Result>
    {
        public ImportBooksCsvCommand(Stream fileStream, string username)
        {
            FileStream = fileStream;
            Username = username;
        }
        public string Username { get; set; }
        public Stream FileStream { get; }
    }
}
