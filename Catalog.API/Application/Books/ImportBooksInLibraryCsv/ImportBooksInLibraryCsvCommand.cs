using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Books.ImportBooksInLibraryCsv
{
    public class ImportBooksInLibraryCsvCommand : CommandBase<Result>
    {
        public ImportBooksInLibraryCsvCommand(string username, Stream fileStream, Guid libraryId)
        {
            UserName = username;
            FileStream = fileStream;
            LibraryId = libraryId;
        }
        public string UserName { get; set; }
        public Stream FileStream { get; }
        public Guid LibraryId { get; set; }
    }
}
