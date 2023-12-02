using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Books.ImportBooksInLibraryXml
{
    public class ImportBooksInLibraryXmlCommand : CommandBase<Result>
    {
        public ImportBooksInLibraryXmlCommand(string username, Stream stream, Guid libraryId)
        {
            Username = username;
            Stream = stream;
            LibraryId = libraryId;
        }
        public string Username { get; set; }
        public Stream Stream { get; set; }
        public Guid LibraryId { get; set; }
    }
}
