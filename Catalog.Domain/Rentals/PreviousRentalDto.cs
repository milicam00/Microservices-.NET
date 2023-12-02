namespace Catalog.Domain.Rentals
{
    public class PreviousRentalDto
    {
        public Guid RentalId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool Returned { get; set; }
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public List<PreviousRentalBookDto> Books { get; set; }
    }
}
