using BuildingBlocks.Domain;

namespace UserAccess.Domain.APIKeys
{
    public class APIKey : Entity
    {

        public Guid KeyId { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }

        public APIKey(string username, string name)
        {
            KeyId = Guid.NewGuid();
            Key = GenerateUniqueKey();
            Username = username;
            CreatedAt = DateTime.Now;
            Name = name;
        }

        public static APIKey CreateKey(string username, string name)
        {
            return new APIKey(username, name);
        }
        private string GenerateUniqueKey()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(0, 16);
        }
    }
}
