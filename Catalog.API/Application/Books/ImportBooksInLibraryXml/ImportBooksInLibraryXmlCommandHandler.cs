using BuildingBlocks.Application.XmlGeneration;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Domain.Libraries;
using Catalog.Domain.Owners;
using Catalog.Infrastructure.Configuration.Commands;

namespace Catalog.API.Application.Books.ImportBooksInLibraryXml
{
    public class ImportBooksInLibraryXmlCommandHandler : ICommandHandler<ImportBooksInLibraryXmlCommand, Result>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IXmlGenerationService _xmlGenerationService;
        private readonly ILibraryRepository _libraryRepository;
        public ImportBooksInLibraryXmlCommandHandler(IBookRepository bookRepository, IOwnerRepository ownerRepository, IXmlGenerationService xmlGenerationService, ILibraryRepository libraryRepository)
        {
            _bookRepository = bookRepository;
            _ownerRepository = ownerRepository;
            _xmlGenerationService = xmlGenerationService;
            _libraryRepository = libraryRepository;
        }
        public async Task<Result> Handle(ImportBooksInLibraryXmlCommand request, CancellationToken cancellationToken)
        {
            Owner owner = await _ownerRepository.GetByUsername(request.Username);
            if (owner == null)
            {
                return Result.Failure("This owner does not exist.");
            }

            List<Book> books = new List<Book>();
            List<Guid> libraryIds = await _libraryRepository.GetLibraryIdsByOwnerId(owner.OwnerId);

            if (!libraryIds.Contains(request.LibraryId))
            {
                return Result.Failure("Only owner of library can add book.");
            }

            List<ImportedBookForOneLibraryDto> importedBooks = _xmlGenerationService.DeserializeBooksForOneLibraryFromXml(request.Stream);
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
