using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Books.GetAllBooks
{
    public class GetAllBooks : CommandBase<PaginationResult<Book>>
    {
        public PaginationFilter PaginationFilter { get; set; }
        public GetAllBooks(PaginationFilter paginationFilter)
        {
            PaginationFilter = paginationFilter;
        }
    }
}
