namespace Catalog.Domain.Rentals
{
    public interface IRentalRepository
    {
        Task AddAsync(Rental rental);
        Task<Rental> GetByIdAsync(Guid rentalId);
        Task<Rental> GeyByUserIdAsync(Guid userId);
        Task<List<Rental>> GetOverdueRentals(DateTime overdueDate);
        void Update(Rental rental);


    }
}
