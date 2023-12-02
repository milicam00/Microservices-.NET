using BuildingBlocks.Application.XmlGeneration;
using BuildingBlocks.Domain.Results;
using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Domain.Libraries;
using Catalog.Domain.Owners;
using Catalog.Infrastructure.Configuration.Commands;

namespace Catalog.API.Application.Books.ImportBooksXml
{
    public class ImportBooksXmlCommandHandler : ICommandHandler<ImportBooksXmlCommand, Result>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IXmlGenerationService _xmlGenerationService;
        private readonly ILibraryRepository _libraryRepository;
        public ImportBooksXmlCommandHandler(IBookRepository bookRepository, IOwnerRepository ownerRepository, IXmlGenerationService xmlGenerationService, ILibraryRepository libraryRepository)
        {
            _bookRepository = bookRepository;
            _ownerRepository = ownerRepository;
            _xmlGenerationService = xmlGenerationService;
            _libraryRepository = libraryRepository;
        }

        public async Task<Result> Handle(ImportBooksXmlCommand request, CancellationToken cancellationToken)
        {
            Owner owner = await _ownerRepository.GetByUsername(request.Username);
            if (owner == null)
            {
                return Result.Failure("This owner does not exist.");
            }
            List<Book> books = new List<Book>();
            List<Guid> libraryIds = await _libraryRepository.GetLibraryIdsByOwnerId(owner.OwnerId);
            List<BookOfOwnerDto> importedBooks = _xmlGenerationService.DeserializeBooksFromXml(request.Stream);
            foreach (var bookDto in importedBooks)
            {
                if (!libraryIds.Contains(bookDto.LibraryId))
                {
                    return Result.Failure("Only owner of library can add book.");
                }

                var book = Book.Create(
                    bookDto.Title,
                    bookDto.Description,
                    bookDto.Author,
                    bookDto.Pages,
                    (Genres)bookDto.Genres,
                    bookDto.PubblicationYear,
                    bookDto.NumberOfCopies,
                    bookDto.LibraryId
                );
                books.Add(book);
            }

            await _bookRepository.AddBooksAsync(books);
            return Result.Success(importedBooks);
        }
    }
}
