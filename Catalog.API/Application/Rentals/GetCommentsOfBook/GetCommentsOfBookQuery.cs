using BuildingBlocks.Domain;
using Catalog.Infrastructure.Configuration.Queries;

namespace Catalog.API.Application.Rentals.GetCommentsOfBook
{
    public class GetCommentsOfBookQuery : QueryBase<Result>
    {
        public GetCommentsOfBookQuery(Guid bookId, int pageNumber, int pageSize, string? orderBy)
        {
            BookId = bookId;
            PageNumber = pageNumber;
            PageSize = pageSize;
            OrderBy = orderBy;

        }
        public Guid BookId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? OrderBy { get; set; }
    }
}
