namespace Catalog.Domain.Rentals
{
    public class PreviousRentalOwnerDto
    {
        public string LibraryName { get; set; }
        public string BookName { get; set; }
        public List<string> BookNames { get; set; }
        public Guid RentalId { get; set; }
        public DateTime RentalDate { get; set; }
        public bool Returned { get; set; }
        public string UserName { get; set; }
    }
}
