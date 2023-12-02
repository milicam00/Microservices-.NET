namespace Catalog.API.Controllers.Requests
{
    public class SearchUnreturnedBooksRequest
    {
        public string? LibraryName { get; set; }
        public string? BookTitle { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? OrderBy { get; set; }
    }
}
