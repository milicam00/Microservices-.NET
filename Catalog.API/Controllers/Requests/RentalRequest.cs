namespace Catalog.API.Controllers.Requests
{
    public class RentalRequest
    {
        public Guid UserId { get; set; }
        public List<Guid> BookIds { get; set; }

    }
}
