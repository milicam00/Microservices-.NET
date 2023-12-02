using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Domain.OwnerRentals;

namespace Catalog.Domain.OwnerRentalBooks
{
    public class OwnerRentalBook : Entity
    {
        public Guid OwnerRentalBookId { get; set; }
        public Guid OwnerRentalId { get; set; }
        public Guid BookId { get; set; }
        public OwnerRental OwnerRental { get; set; }
        public Book Book { get; set; }
        public OwnerRentalBook()
        {
            OwnerRentalBookId = Guid.NewGuid();
        }

        public OwnerRentalBook(Guid bookId)
        {
            BookId = bookId;
        }

    }
}
