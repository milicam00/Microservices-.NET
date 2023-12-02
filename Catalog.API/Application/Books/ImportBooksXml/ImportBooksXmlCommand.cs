using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Books.ImportBooksXml
{
    public class ImportBooksXmlCommand : CommandBase<Result>
    {
        public ImportBooksXmlCommand(string username, Stream stream)
        {
            Username = username;
            Stream = stream;
        }
        public string Username { get; set; }
        public Stream Stream { get; set; }
    }
}
