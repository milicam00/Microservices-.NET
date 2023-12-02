using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Domain.Libraries;
using Catalog.Infrastructure.Configuration.Commands;

namespace Catalog.API.Application.Books.CreateBook
{
    public class CreateBookCommandHandler : ICommandHandler<CreateBookCommand, Result>
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILibraryRepository _libraryRepository;

        public CreateBookCommandHandler(IBookRepository bookRepository, ILibraryRepository libraryRepository)
        {
            _bookRepository = bookRepository;
            _libraryRepository = libraryRepository;
        }

        public async Task<Result> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {

            var library = await _libraryRepository.GetByIdAsync(request.LibraryId);
            if (library == null)
            {
                return Result.Failure("This library does not exist.");
            }

            var book = Book.Create(
                request.Title,
                request.Description,
                request.Author,
                request.Pages,
                (Genres)request.Genres,
                request.PubblicationYear,
                request.NumberOfCopies,
                request.LibraryId
                );

            await _bookRepository.AddAsync(book);

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
