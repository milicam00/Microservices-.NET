namespace Catalog.Domain.Rentals
{
    public class UnreturnedBooksDto
    {
        public string LibraryName { get; set; }
        public Guid BookId { get; set; }
        public string BookTitle { get; set; }
        public string UserName { get; set; }
        public string RentalDate { get; set; }
        public bool Returned { get; set; }
    }
}
