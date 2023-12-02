using BuildingBlocks.Domain;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using UserAccess.API.Application.Authentication;
using UserAccess.Domain.RefreshTokens;
using UserAccess.Domain.Users;
using UserAccess.Infrastructure.Configuration.Commands;

namespace UserAccess.API.Application.TokenRefresh
{
    public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, Result>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        public RefreshTokenCommandHandler(IRefreshTokenRepository refreshToken, IUserRepository userRepository)
        {
            _refreshTokenRepository = refreshToken;
            _userRepository = userRepository;
        }
        public async Task<Result> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {

            RefreshToken refreshToken = await _refreshTokenRepository.GetByToken(request.RefreshToken);
            Guid userId = refreshToken.UserId;
            User user = await _userRepository.GetByIdAsync(userId);
            if (refreshToken == null)
            {
                return Result.Failure("Refresh token does not exist.");
            }
            if (!refreshToken.IsActive)
            {
                return Result.Failure("Token is not active.");
            }

            RefreshToken newRefreshToken = rotateRefreshToken(refreshToken, userId);
            await _refreshTokenRepository.AddAsync(newRefreshToken);

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("abvgdasdlsadasdasdadasd1"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.Email,user.Email),
                };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Value));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = "your_publisher",
                Audience = "your_public",
                Expires = DateTime.Now.AddMinutes(20),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            AuthenticationResult result = new AuthenticationResult(tokenHandler.WriteToken(token), newRefreshToken.Token);
            return Result.Success(result);

        }

        private string GenerateRefreshToken()
        {
            var randomNum = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNum);
                return Convert.ToBase64String(randomNum);
            }
        }

        private RefreshToken rotateRefreshToken(RefreshToken refreshToken, Guid userId)
        {
            RefreshToken newRefreshToken = RefreshToken.Create(GenerateRefreshToken(), userId);
            revokeRefreshToken(refreshToken, newRefreshToken);
            return newRefreshToken;
        }
        private void revokeRefreshToken(RefreshToken token, RefreshToken replacedByToken)
        {
            token.Revoked = DateTime.UtcNow;
            token.ReplacedByToken = replacedByToken.Token;
        }
    }
}
