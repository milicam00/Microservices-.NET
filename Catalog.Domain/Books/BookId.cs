using BuildingBlocks.Domain;

namespace Catalog.Domain.Books
{
    public class BookId : TypedIdValueBase
    {
        public BookId(Guid value) : base(value)
        {
        }
    }
}
