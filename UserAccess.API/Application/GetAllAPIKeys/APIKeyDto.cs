namespace UserAccess.API.Application.GetAllAPIKeys
{
    public class APIKeyDto
    {
        public Guid APIKeyId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }

    }
}
