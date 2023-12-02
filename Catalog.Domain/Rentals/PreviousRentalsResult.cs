namespace Catalog.Domain.Rentals
{
    public class PreviousRentalsResult
    {
        public Guid RentalId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool IsReturned { get; set; }
        public List<PreviousRentalBookDto> Books { get; set; }
    }
}
