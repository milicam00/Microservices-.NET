namespace Catalog.Domain.Rentals
{
    public class ReturnDto
    {
        public Guid RentalId { get; set; }
        public Guid ReaderId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime? ReturnDate { get; set; }

    }
}
