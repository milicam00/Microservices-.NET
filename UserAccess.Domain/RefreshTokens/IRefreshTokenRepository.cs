namespace UserAccess.Domain.RefreshTokens
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken token);
        Task<RefreshToken> GetByToken(string token);
        void RemoveAsync(RefreshToken token);
        Task<List<RefreshToken>> GetRefreshTokensByUser(Guid userId);
    }
}
