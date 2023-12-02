using BuildingBlocks.Domain;
using UserAccess.Infrastructure.Contracts;

namespace UserAccess.API.Application.TokenRefresh
{
    public class RefreshTokenCommand : CommandBase<Result>
    {
        public RefreshTokenCommand(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
        public string RefreshToken { get; set; }
    }
}
