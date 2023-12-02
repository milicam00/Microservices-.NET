using Microsoft.EntityFrameworkCore;
using UserAccess.Domain.APIKeys;

namespace UserAccess.Infrastructure.Domain.APIKeys
{
    public class APIKeyRepository : IAPIKeyRepository
    {
        private readonly UserAccessContext _userAccessContext;
        public APIKeyRepository(UserAccessContext userAccessContext)
        {
            _userAccessContext = userAccessContext;
        }
        public async Task AddAsync(APIKey apiKey)
        {
            await _userAccessContext.APIKeys.AddAsync(apiKey);
        }

        public async Task<APIKey?> GetAsync(string username)
        {
            return await _userAccessContext.APIKeys.Where(x => x.Username == username).FirstOrDefaultAsync();
        }

        public async Task<APIKey?> GetByIdAsync(Guid apiKeyId)
        {
            return await _userAccessContext.APIKeys.Where(x => x.KeyId == apiKeyId).FirstOrDefaultAsync();
        }

        public void Update(APIKey apiKey)
        {
            _userAccessContext.APIKeys.Update(apiKey);
        }

        public void DeleteApiKey(APIKey apiKey)
        {
            _userAccessContext.APIKeys.Remove(apiKey);
        }
    }
}
