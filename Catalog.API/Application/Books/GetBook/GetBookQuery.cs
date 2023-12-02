using BuildingBlocks.Domain;
using Catalog.Infrastructure.Configuration.Queries;

namespace Catalog.API.Application.Books.GetBook
{
    public class GetBookQuery : QueryBase<Result>
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int? PubblicationYear { get; set; }
        public int? Genres { get; set; }
        public int? Pages { get; set; }
        public int? Rate { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? OrderBy { get; set; }

        public GetBookQuery(string? title, string? author, int? pubblicationYear, int? genre, int? numberOfPages, int? rate, int pageNumber, int pageSize, string? orderBy)
        {
            Title = title;
            Author = author;
            PubblicationYear = pubblicationYear;
            Genres = genre;
            Pages = numberOfPages;
            PageNumber = pageNumber;
            PageSize = pageSize;
            Rate = rate;
            OrderBy = orderBy;
        }
    }
}
