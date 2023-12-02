namespace UserAccess.Domain.APIKeys
{
    public interface IAPIKeyRepository
    {
        Task AddAsync(APIKey apiKey);
        Task<APIKey?> GetAsync(string username);
        Task<APIKey?> GetByIdAsync(Guid apiKeyId);
        void Update(APIKey apiKey);
        void DeleteApiKey(APIKey apiKey);
    }
}
