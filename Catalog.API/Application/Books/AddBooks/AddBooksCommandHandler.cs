using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Domain.Libraries;
using Catalog.Domain.Owners;
using Catalog.Infrastructure.Configuration.Commands;

namespace Catalog.API.Application.Books.AddBooks
{
    public class AddBooksCommandHandler : ICommandHandler<AddBooksCommand, Result>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly ILibraryRepository _libraryRepository;
        public AddBooksCommandHandler(IBookRepository bookRepository, IOwnerRepository ownerRepository, ILibraryRepository libraryRepository)
        {
            _bookRepository = bookRepository;
            _ownerRepository = ownerRepository;
            _libraryRepository = libraryRepository;
        }
        public async Task<Result> Handle(AddBooksCommand request, CancellationToken cancellationToken)
        {
            Owner owner = await _ownerRepository.GetByUsername(request.OwnerUsername);
            if (owner == null)
            {
                return Result.Failure("This owner does not exist.");
            }

            List<Book> books = new List<Book>();
            List<Guid> libraryIds = await _libraryRepository.GetLibraryIdsByOwnerId(owner.OwnerId);
            foreach (var bookDto in request.BookList)
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

            return Result.Success(request.BookList);

        }
    }
}
