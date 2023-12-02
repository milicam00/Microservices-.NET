namespace Catalog.Domain.OwnerRentals
{
    public interface IOwnerRentalRepository
    {
        Task AddAsync(OwnerRental ownerRental);
    }
}
