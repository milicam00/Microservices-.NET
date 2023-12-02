namespace Catalog.API.Controllers.Requests
{
    public class BookSearchRequest
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

    }
}
