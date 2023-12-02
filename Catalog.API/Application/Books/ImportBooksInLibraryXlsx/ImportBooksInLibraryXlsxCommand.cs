using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Books.ImportBooksInLibraryXlsx
{
    public class ImportBooksInLibraryXlsxCommand : CommandBase<Result>
    {
        public ImportBooksInLibraryXlsxCommand(string username, Stream stream, Guid libraryId)
        {
            Username = username;
            FileStream = stream;
            LibraryId = libraryId;
        }
        public string Username { get; set; }
        public Stream FileStream { get; set; }
        public Guid LibraryId { get; set; }
    }
}
