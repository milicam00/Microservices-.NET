using BuildingBlocks.Domain;
using Catalog.Infrastructure.Configuration.Queries;

namespace Catalog.API.Application.Books.GetBookOfLibrary
{
    public class GetBooksOfLibraryQuery : QueryBase<Result>
    {
        public GetBooksOfLibraryQuery(Guid libraryId, string? title, string? author, int? pubblicationYear, int? genre, int? pages, int? rate, int pageNumber, int pageSize, string? orderBy)
        {
            LibraryId = libraryId;
            Title = title;
            Author = author;
            PubblicationYear = pubblicationYear;
            Genres = genre;
            Pages = pages;
            Rate = rate;
            PageNumber = pageNumber;
            PageSize = pageSize;
            OrderBy = orderBy;
        }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public int? PubblicationYear { get; set; }
        public int? Genres { get; set; }
        public int? Pages { get; set; }
        public int? Rate { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Guid LibraryId { get; set; }
        public string? OrderBy { get; set; }
    }
}
