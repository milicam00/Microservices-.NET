using BuildingBlocks.Domain;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using UserAccess.Domain.RefreshTokens;
using UserAccess.Domain.Users;
using UserAccess.Infrastructure.Configuration.Commands;
using UserAccess.Infrastructure.Domain.Users;

namespace UserAccess.API.Application.Authentication
{
    public class AuthenticateCommandHandler : ICommandHandler<AuthenticateCommand, Result>
    {

        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public AuthenticateCommandHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<Result> Handle(AuthenticateCommand command, CancellationToken cancellationToken)
        {

            User user = await _userRepository.GetByUsernameAsync(command.UserName);

            if (user == null)
            {
                return Result.Failure("This user does not exist.");
            }

            if (!user.IsActive)
            {
                return Result.Failure("User is not active!");
            }

            if (!PasswordManager.VerifyHashedPassword(user.Password, command.Password))
            {
                return Result.Failure("Incorect login or password");
            }

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

            var identity = new ClaimsIdentity(claims, "custom");
            var principal = new ClaimsPrincipal(identity);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = "your_publisher",
                Audience = "your_public",
                Expires = DateTime.Now.AddMinutes(10),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            RefreshToken refreshToken = RefreshToken.Create(GenerateRefreshToken(), user.UserId);
            await _refreshTokenRepository.AddAsync(refreshToken);

            AuthenticationResult result = new AuthenticationResult(tokenHandler.WriteToken(token), refreshToken.Token);
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


    }
}
