using BuildingBlocks.Domain;
using Catalog.Infrastructure.Configuration.Queries;

namespace Catalog.API.Application.Rentals.GetReportedComments
{
    public class GetReportedCommentsQuery : QueryBase<Result>
    {
        public GetReportedCommentsQuery(int pageNumber, int pageSize, string? orderBy)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            OrderBy = orderBy;
        }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? OrderBy { get; set; }
    }
}
