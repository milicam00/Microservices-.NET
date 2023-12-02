namespace Catalog.API.Controllers.Requests
{
    public class CommentSearchRequest
    {
        public string? LibraryName { get; set; }
        public string? BookTitle { get; set; }
        public string? UserName { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? OrderBy { get; set; }
    }
}
