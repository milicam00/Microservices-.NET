namespace Catalog.Domain.Owners
{
    public class OwnerDto
    {
        public Guid OwnerId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LibraryName { get; set; }
    }
}
