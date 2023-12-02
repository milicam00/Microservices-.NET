using BuildingBlocks.Application.ICsvGeneration;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Domain.Libraries;
using Catalog.Domain.Owners;
using Catalog.Infrastructure.Configuration.Commands;

namespace Catalog.API.Application.Books.ImportBooksInLibraryCsv
{
    public class ImportBooksInLibraryCsvCommandHandler : ICommandHandler<ImportBooksInLibraryCsvCommand, Result>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICsvGenerationService _csvGenerationService;
        private readonly ILibraryRepository _libraryRepository;
        public ImportBooksInLibraryCsvCommandHandler(IBookRepository bookRepository, IOwnerRepository ownerRepository, ICsvGenerationService csvGenerationService, ILibraryRepository libraryRepository)
        {
            _bookRepository = bookRepository;
            _ownerRepository = ownerRepository;
            _csvGenerationService = csvGenerationService;
            _libraryRepository = libraryRepository;
        }
        public async Task<Result> Handle(ImportBooksInLibraryCsvCommand request, CancellationToken cancellationToken)
        {
            Owner owner = await _ownerRepository.GetByUsername(request.UserName);
            if (owner == null)
            {
                return Result.Failure("This owner does not exist.");
            }

            List<Guid> libraryIds = await _libraryRepository.GetLibraryIdsByOwnerId(owner.OwnerId);
            if (!libraryIds.Contains(request.LibraryId))
            {
                return Result.Failure("Only owner of library can add book.");
            }

            List<Book> books = new List<Book>();
            List<ImportedBookForOneLibraryDto> importedBooks = await _csvGenerationService.DeserializeBooksFromCsvForOneLibrary(request.FileStream);
            foreach (var bookDto in importedBooks)
            {
                var book = Book.Create(
                       bookDto.Title,
                       bookDto.Description,
                       bookDto.Author,
                       bookDto.Pages,
                       (Genres)bookDto.Genres,
                       bookDto.PubblicationYear,
                       bookDto.NumberOfCopies,
                       request.LibraryId
                       );
                books.Add(book);
            }
            await _bookRepository.AddBooksAsync(books);
            return Result.Success(books);
        }
    }
}
