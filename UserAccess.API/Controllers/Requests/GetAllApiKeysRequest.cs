namespace UserAccess.API.Controllers.Requests
{
    public class GetAllApiKeysRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Name { get; set; }
    }
}
