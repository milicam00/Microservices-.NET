using BuildingBlocks.Domain;
using UserAccess.Domain.Users;

namespace UserAccess.Domain.RefreshTokens
{
    public class RefreshToken : Entity
    {
        public Guid TokenId { get; set; }
        public string Token { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }
        public DateTime? Revoked { get; set; }
        public string? ReplacedByToken { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsRevoked => Revoked != null;
        public bool IsActive => !IsRevoked && !IsExpired;
        public User User { get; set; }
        public Guid UserId { get; set; }

        public RefreshToken()
        {
            TokenId = Guid.NewGuid();
        }

        public RefreshToken(string token, Guid userId)
        {
            Token = token;
            Created = DateTime.UtcNow;
            Expires = DateTime.UtcNow.AddMinutes(20);
            UserId = userId;
        }

        public static RefreshToken Create(string token, Guid userId)
        {
            return new RefreshToken(token, userId);
        }

    }
}
