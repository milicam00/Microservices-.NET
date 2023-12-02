using Catalog.Domain.Rentals;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Domain.Rentals
{
    public class RentalRepository : IRentalRepository
    {
        private readonly CatalogContext _catalogContext;

        public RentalRepository(CatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public async Task AddAsync(Rental rental)
        {
            await _catalogContext.Rentals.AddAsync(rental);
        }

        public async Task<Rental?> GetByIdAsync(Guid rentalId)
        {
            return await _catalogContext.Rentals.Where(x => x.RentalId == rentalId).FirstOrDefaultAsync();
        }

        public async Task<Rental?> GeyByUserIdAsync(Guid userId)
        {
            return await _catalogContext.Rentals.Where(x => x.ReaderId == userId).FirstOrDefaultAsync();
        }
        public async Task<List<Rental>> GetOverdueRentals(DateTime overdueDate)
        {
            return await _catalogContext.Rentals
                .Where(rental => !rental.ReturnDate.HasValue && rental.RentalDate <= overdueDate)
                .ToListAsync();
        }

        public void Update(Rental rental)
        {
            _catalogContext.Rentals.Update(rental);
        }


    }
}
