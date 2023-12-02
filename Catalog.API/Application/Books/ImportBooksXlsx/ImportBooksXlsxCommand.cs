using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Books.ImportBooksXlsx
{
    public class ImportBooksXlsxCommand : CommandBase<Result>
    {
        public ImportBooksXlsxCommand(string username, Stream stream)
        {
            Username = username;
            FileStream = stream;
        }
        public string Username { get; set; }
        public Stream FileStream { get; set; }
    }
}
