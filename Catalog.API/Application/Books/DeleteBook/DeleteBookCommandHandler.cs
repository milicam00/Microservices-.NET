using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Domain.Libraries;
using Catalog.Domain.Owners;
using Catalog.Infrastructure.Configuration.Commands;

namespace Catalog.API.Application.Books.DeleteBook
{
    public class DeleteBookCommandHandler : ICommandHandler<DeleteBookCommand, Result>
    {

        private readonly IBookRepository _bookRepository;
        private readonly ILibraryRepository _libraryRepository;
        private readonly IOwnerRepository _ownerRepository;
        public DeleteBookCommandHandler(IBookRepository bookRepository, ILibraryRepository libraryRepository, IOwnerRepository ownerRepository)
        {
            _bookRepository = bookRepository;
            _libraryRepository = libraryRepository;
            _ownerRepository = ownerRepository;
        }

        public async Task<Result> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            Book book = await _bookRepository.GetByIdAsync(request.BookId);
            if (book == null)
            {
                return Result.Failure("This book does not exist.");
            }
            Library? library = await _libraryRepository.GetByIdAsync(book.LibraryId);
            Owner owner = await _ownerRepository.GetByUsername(request.OwnerUsername);
            if (owner == null)
            {
                return Result.Failure("This owner does not exist.");
            }
            if (library?.OwnerId == owner.OwnerId)
            {
                book.DeleteBook();
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

            return Result.Failure("Only the library owner who added the book can delete it.");

        }
    }
}
