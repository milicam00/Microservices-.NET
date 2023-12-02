using BuildingBlocks.Domain;
using UserAccess.Domain.RefreshTokens;
using UserAccess.Domain.Users;
using UserAccess.Infrastructure.Configuration.Commands;

namespace UserAccess.API.Application.Logout
{
    public class LogoutCommandHandler : ICommandHandler<LogoutCommand, Result>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        public LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
        }
        public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {

            RefreshToken refreshToken = await _refreshTokenRepository.GetByToken(request.RefreshToken);
            if (refreshToken == null)
            {
                return Result.Failure("Refresh token does not exist.");
            }

            Guid userId = refreshToken.UserId;
            User user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return Result.Failure("User does not exist.");
            }

            List<RefreshToken> tokens = await _refreshTokenRepository.GetRefreshTokensByUser(userId);
            foreach (RefreshToken token in tokens)
            {
                _refreshTokenRepository.RemoveAsync(token);
            }
            return Result.Success("Successfully logout");

        }
    }
}
