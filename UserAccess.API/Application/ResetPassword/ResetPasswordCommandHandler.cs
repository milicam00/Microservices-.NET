using BuildingBlocks.Domain;
using System.IdentityModel.Tokens.Jwt;
using UserAccess.Domain.Users;
using UserAccess.Infrastructure.Configuration.Commands;
using UserAccess.Infrastructure.Domain.Users;

namespace UserAccess.API.Application.ResetPassword
{
    public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, Result>
    {
        private readonly IUserRepository _userRepository;

        public ResetPasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(request.Token) as JwtSecurityToken;
            if (jwtToken == null)
            {
                return Result.Failure("Token is not valid.");

            }
            var usernameClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "unique_name");
            if(usernameClaim == null)
            {
                return Result.Failure("Username claim does not exist.");
            }
            User user = await _userRepository.GetByUsernameAsync(usernameClaim.Value);
            if (user != null)
            {
                if (user.ResetPasswordCode == request.Code && user.ResetPasswordCodeExpiration < DateTime.Now.AddMinutes(20))
                {
                    var newPassword = PasswordManager.HashPassword(request.NewPassword);
                    user.Password = newPassword;
                    _userRepository.UpdateUser(user);
                }
                return Result.Success("Password has been reset.");
            }
            return Result.Failure("User does not exist.");

        }
    }
}
