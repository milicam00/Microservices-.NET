using BuildingBlocks.Application.Emails;
using BuildingBlocks.Domain;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserAccess.Domain.Users;
using UserAccess.Infrastructure.Configuration.Commands;

namespace UserAccess.API.Application.ResetPasswordRequest
{
    public class ResetPasswordRequestCommandHandler : ICommandHandler<ResetPasswordRequestCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public ResetPasswordRequestCommandHandler(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }
        public async Task<Result> Handle(ResetPasswordRequestCommand request, CancellationToken cancellationToken)
        {

            Random random = new Random();
            User user = await _userRepository.GetByUsernameAsync(request.Username);
            int resetCode = random.Next(10000, 100000);

            if (user == null)
            {
                return Result.Failure("User does not exist.");
            }
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("abvgdasdlsadasdasdadasd1"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName)

                };

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
            string userToken = tokenHandler.WriteToken(token);
            string resetLink = $"https://localhost:44319/api/userAccess/reset-password?code={resetCode}&token={userToken}";


            user.ResetPasswordCode = resetCode;
            user.ResetPasswordCodeExpiration = DateTime.UtcNow.AddMinutes(20);
            _userRepository.UpdateUser(user);

            await _emailService.SendEmailAsync(user.Email, "Password reset request", "Click on the link to reset password: " + resetLink + "\n");

            return Result.Success(resetLink);

        }
    }
}
