using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Domain.RentalBooks;
using Catalog.Domain.Rentals;
using Catalog.Infrastructure.Configuration.Commands;

namespace Catalog.API.Application.Rentals.RateBook
{
    public class RateBookCommandHandler : ICommandHandler<RateBookCommand, Result>
    {
        public readonly IRentalBookRepository _rentalRepository;
        public readonly IBookRepository _bookRepository;
        public RateBookCommandHandler(IRentalBookRepository rentalRepository, IBookRepository bookRepository)
        {
            _rentalRepository = rentalRepository;
            _bookRepository = bookRepository;
        }

        public async Task<Result> Handle(RateBookCommand command, CancellationToken cancellationToken)
        {

            RentalBook? rental = await _rentalRepository.GetByIdAsync(command.RentalBookId);
            if (rental != null)
            {
                rental.RateBook(command.Rate, command.Text);
                Book book = await _bookRepository.GetByIdAsync(rental.BookId);
                book.NumberOfRatings++;
                double newAverageRating = ((book.UserRating * (book.NumberOfRatings - 1)) + command.Rate) / (book.NumberOfRatings);
                book.UserRating = newAverageRating;
                _bookRepository.UpdateBook(book);

                var retedBookDto = new RatedBookDto
                {
                    BookTitle = book.Title,
                    Rate = command.Rate,
                    Text = command.Text
                };

                return Result.Success(retedBookDto);
            }


            return Result.Failure("This rental does not exist");

        }
    }
}
