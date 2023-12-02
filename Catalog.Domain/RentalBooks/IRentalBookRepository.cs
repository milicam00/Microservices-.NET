using Catalog.Domain.Owners;
using Catalog.Domain.Readers;

namespace Catalog.Domain.RentalBooks
{
    public interface IRentalBookRepository
    {
        Task AddAsync(RentalBook rentalBook);

        Task<RentalBook?> GetByIdAsync(Guid rentalBookId);

        Task<RentalBook?> GetByRentalIdAsync(Guid rentalId);

        Task<List<RentalBook>> GetRentalBooks(Guid rentalId);
        void UpdateRentalBook(RentalBook rentalBook);
        Task<Reader> GetReader(Guid rentalId);
        Task<Owner> GetOwner(Guid bookId);
    }
}
