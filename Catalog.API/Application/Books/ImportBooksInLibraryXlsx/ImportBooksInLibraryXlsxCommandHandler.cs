using BuildingBlocks.Application.IXlsxGeneration;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Domain.Libraries;
using Catalog.Domain.Owners;
using Catalog.Infrastructure.Configuration.Commands;

namespace Catalog.API.Application.Books.ImportBooksInLibraryXlsx
{
    public class ImportBooksInLibraryXlsxCommandHandler : ICommandHandler<ImportBooksInLibraryXlsxCommand, Result>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IXlsxGenerationService _xlsxGenerationService;
        private readonly ILibraryRepository _libraryRepository;
        public ImportBooksInLibraryXlsxCommandHandler(IBookRepository bookRepository, IOwnerRepository ownerRepository, IXlsxGenerationService xlsxGenerationService, ILibraryRepository libraryRepository)
        {
            _bookRepository = bookRepository;
            _ownerRepository = ownerRepository;
            _xlsxGenerationService = xlsxGenerationService;
            _libraryRepository = libraryRepository;
        }

        public async Task<Result> Handle(ImportBooksInLibraryXlsxCommand request, CancellationToken cancellationToken)
        {
            Owner owner = await _ownerRepository.GetByUsername(request.Username);
            if (owner == null)
            {
                return Result.Failure("This owner does not exist.");
            }

            List<Book> books = new List<Book>();
            List<ImportedBookForOneLibraryDto> importedBooks = await _xlsxGenerationService.DeserializeBooksForOneLibraryFromXlsx(request.FileStream);
            List<Guid> libraryIds = await _libraryRepository.GetLibraryIdsByOwnerId(owner.OwnerId);
            if (!libraryIds.Contains(request.LibraryId))
            {
                return Result.Failure("Only owner of library can add book.");
            }
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
