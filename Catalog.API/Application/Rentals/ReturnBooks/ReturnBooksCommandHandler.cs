using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Domain.Readers;
using Catalog.Domain.RentalBooks;
using Catalog.Domain.Rentals;
using Catalog.Infrastructure.Configuration.Commands;

namespace Catalog.API.Application.Rentals.ReturnBooks
{
    public class ReturnBooksCommandHandler : ICommandHandler<ReturnBooksCommand, Result>
    {

        private readonly IRentalRepository _rentalRepository;
        private readonly IRentalBookRepository _rentalBooksRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IReaderRepository _readerRepository;
        public ReturnBooksCommandHandler(IRentalRepository rentalRepository, IRentalBookRepository rentalBooksRepository, IBookRepository bookRepository, IReaderRepository readerRepository)
        {
            _rentalRepository = rentalRepository;
            _rentalBooksRepository = rentalBooksRepository;
            _bookRepository = bookRepository;
            _readerRepository = readerRepository;
        }
        public async Task<Result> Handle(ReturnBooksCommand request, CancellationToken cancellationToken)
        {
            Reader reader = await _readerRepository.GetByUsername(request.ReaderUsername);
            if (reader == null)
            {
                return Result.Failure("This reader does not exist.");
            }
            Rental rental = await _rentalRepository.GeyByUserIdAsync(reader.ReaderId);

            if (rental == null)
            {
                return Result.Failure("This rental does not exist.");
            }

            if (rental.Returned == true)
            {
                return Result.Failure("This reader has returned all books.");
            }

            List<RentalBook> rentalBooks = await _rentalBooksRepository.GetRentalBooks(rental.RentalId);

            foreach (var rentalBook in rentalBooks)
            {
                Book book = await _bookRepository.GetByIdAsync(rentalBook.BookId);
                book.NumberOfCopies++;
                _bookRepository.UpdateBook(book);
            }

            rental.ReturnBooks();
            _rentalRepository.Update(rental);

            var returnDto = new ReturnDto
            {
                RentalId = rental.RentalId,
                ReaderId = rental.ReaderId,
                RentalDate = rental.RentalDate,
                ReturnDate = rental.ReturnDate
            };

            return Result.Success(returnDto);
        }
    }
}
