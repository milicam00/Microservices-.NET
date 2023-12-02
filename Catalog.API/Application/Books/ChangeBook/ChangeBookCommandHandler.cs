using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Domain.Libraries;
using Catalog.Domain.Owners;
using Catalog.Infrastructure.Configuration.Commands;
using BookDto = Catalog.Domain.Books.BookDto;

namespace Catalog.API.Application.Books.ChangeBook
{
    public class ChangeBookCommandHandler : ICommandHandler<ChangeBookCommand, Result>
    {
        public readonly IBookRepository _bookRepository;
        public readonly ILibraryRepository _libraryRepository;
        private readonly IOwnerRepository _ownerRepository;
        public ChangeBookCommandHandler(IBookRepository bookRepository, ILibraryRepository libraryRepository, IOwnerRepository ownerRepository)
        {
            _bookRepository = bookRepository;
            _libraryRepository = libraryRepository;
            _ownerRepository = ownerRepository;
        }

        public async Task<Result> Handle(ChangeBookCommand request, CancellationToken cancellationToken)
        {
            Book book = await _bookRepository.GetByIdAsync(request.BookId);
            if (book == null)
            {
                return Result.Failure("This book does not exist.");
            }

            Library? library = await _libraryRepository.GetByIdAsync(book.LibraryId);
            if (library == null)
            {
                return Result.Failure("Library does not exist.");
            }
            Owner owner = await _ownerRepository.GetByUsername(request.Username);
            if (library.OwnerId != owner.OwnerId)
            {
                return Result.Failure("Only owner of library can change the book.");
            }

            book.EditBook(request.Title, request.Description, request.Author, request.Pages, request.PubblicationYear, request.UserRating, request.NumberOfCopies);
            _bookRepository.UpdateBook(book);

            var bookDto = new BookDto
            {
                BookId = book.BookId,
                Title = book.Title,
                Description = book.Description,
                Author = book.Author,
                Pages = book.Pages,
                Genres = book.Genres,
                PubblicationYear = book.PubblicationYear,
                UserRating = book.UserRating,
                NumberOfCopies = book.NumberOfCopies
            };
            return Result.Success(bookDto);
        }
    }
}
