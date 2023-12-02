namespace Catalog.API.Controllers.Requests
{
    public class SearchRentalOwnerRequest
    {
        public string? LibraryName { get; set; }
        public bool? IsReturned { get; set; }
        public string? Username { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? OrderBy { get; set; }
    }
}
