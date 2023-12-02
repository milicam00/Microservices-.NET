using Microsoft.EntityFrameworkCore;
using UserAccess.Domain.RefreshTokens;

namespace UserAccess.Infrastructure.Domain.RefreshTokens
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly UserAccessContext _userAccessContext;
        public RefreshTokenRepository(UserAccessContext userAccessContext)
        {
            _userAccessContext = userAccessContext;
        }

        public async Task AddAsync(RefreshToken token)
        {
            await _userAccessContext.RefreshTokens.AddAsync(token);
        }

        public Task<RefreshToken?> GetByToken(string token)
        {
            return _userAccessContext.RefreshTokens.Where(x => x.Token == token).FirstOrDefaultAsync();
        }

        public async Task<List<RefreshToken>> GetRefreshTokensByUser(Guid userId)
        {
            return await _userAccessContext.RefreshTokens.Where(x => x.UserId == userId).ToListAsync();
        }

        public void RemoveAsync(RefreshToken token)
        {
            _userAccessContext.RefreshTokens.Remove(token);
        }
    }
}
