namespace Catalog.Domain.Rentals
{
    public class RentalResult
    {
        public Guid UserId { get; set; }
        public List<RentalDto> RentalDtos { get; set; }
    }
}
