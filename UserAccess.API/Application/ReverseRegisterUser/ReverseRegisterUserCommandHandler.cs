using BuildingBlocks.Domain;
using UserAccess.Domain.Users;
using UserAccess.Infrastructure.Configuration.Commands;

namespace UserAccess.API.Application.ReverseRegisterUser
{
    public class ReverseRegisterUserCommandHandler : ICommandHandler<ReverseRegisterUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        public ReverseRegisterUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Result> Handle(ReverseRegisterUserCommand request, CancellationToken cancellationToken)
        {

            User user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null)
            {
                return Result.Failure("User does not exist.");
            }
            _userRepository.DeleteUser(user);
            return Result.Success("Deleted user");
        }
    }
}
