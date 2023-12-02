using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Domain.Libraries;
using Catalog.Domain.OwnerRentals;
using Catalog.Domain.Owners;
using Catalog.Domain.Rentals;
using Catalog.Infrastructure.Configuration.Commands;

namespace Catalog.API.Application.OwnerRentals.CreateOwnerRental
{
    public class CreateOwnerRentalCommandHandler : ICommandHandler<CreateOwnerRentalCommand, Result>
    {
        private readonly IOwnerRentalRepository _ownerRentalRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IBookRepository _bookRepository;
        private readonly ILibraryRepository _libraryRepository;
        public CreateOwnerRentalCommandHandler(IOwnerRentalRepository ownerRentalRepository, IOwnerRepository ownerRepository, IBookRepository bookRepository, ILibraryRepository libraryRepository)
        {
            _ownerRentalRepository = ownerRentalRepository;
            _ownerRepository = ownerRepository;
            _bookRepository = bookRepository;
            _libraryRepository = libraryRepository;
        }

        public async Task<Result> Handle(CreateOwnerRentalCommand request, CancellationToken cancellationToken)
        {
            var owner = await _ownerRepository.GetByUsername(request.Username);
            if (owner == null)
            {
                return Result.Failure("This owner does not exist.");
            }
            List<Guid> libraryIds = await _libraryRepository.GetLibraryIdsByOwnerId(owner.OwnerId);
            List<Book> rentedBooks = await _bookRepository.GetByIdsAsync(request.BookIds);
            foreach (Book book in rentedBooks)
            {
                if (!libraryIds.Contains(book.LibraryId))
                {
                    return Result.Failure("Owner can only rent books from his libraries.");
                }

            }
            var unvailableBooks = rentedBooks.Where(book => book.IsDeleted || book.NumberOfCopies <= 0).ToList();
            if (unvailableBooks.Any())
            {
                var unvailableBookTitles = unvailableBooks.Select(book => book.Title);
                return Result.Failure($"The following books are deleted: {string.Join(", ", unvailableBooks)}.");
            }
            var rentedBookCounts = request.BookIds.GroupBy(id => id).ToDictionary(group => group.Key, group => group.Count());
            foreach (var bookId in rentedBookCounts.Keys)
            {
                var rentedBook = rentedBooks.SingleOrDefault(book => book.BookId == bookId);
                if (rentedBook == null)
                {
                    return Result.Failure($"Book with ID {bookId} is not selected.");
                }

                var requestedCount = rentedBookCounts[bookId];
                var availableCount = rentedBook.NumberOfCopies;

                if (requestedCount > availableCount)
                {
                    return Result.Failure($"Not enough copies available for book with ID {bookId}.");
                }
            }
            foreach (var rent in rentedBooks)
            {
                rent.NumberOfCopies--;
            }
            _bookRepository.UpdateBooks(rentedBooks);

            var bookInfo = rentedBooks.Select(book => new
            {
                Title = book.Title,
                Author = book.Author,
                LibraryId = book.LibraryId
            }).ToList();

            var libraries = await _libraryRepository.GetByIdsAsync(libraryIds);
            List<RentalDto> rentalDtos = new List<RentalDto>();
            foreach (var info in bookInfo)
            {
                var library = libraries.FirstOrDefault(lib => lib.LibraryId == info.LibraryId);
                if(library == null)
                {
                    return Result.Failure("Library does not exist");
                }
                var rentalDto = new RentalDto
                {
                    BookTitle = info.Title,
                    BookAuthor = info.Author,
                    Library = library.LibraryName
                };
                rentalDtos.Add(rentalDto);

            }

            OwnerRental rental = OwnerRental.Create(owner.OwnerId, request.BookIds);
            await _ownerRentalRepository.AddAsync(rental);

            RentalResult result = new RentalResult
            {
                UserId = owner.OwnerId,
                RentalDtos = rentalDtos
            };
            return Result.Success(result);
        }
    }
}
