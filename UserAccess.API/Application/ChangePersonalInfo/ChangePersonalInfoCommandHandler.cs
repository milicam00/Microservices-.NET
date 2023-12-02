using UserAccess.Domain.Users;
using UserAccess.Infrastructure.Configuration.Commands;

namespace UserAccess.API.Application.ChangePersonalInfo
{
    public class ChangePersonalInfoCommandHandler : ICommandHandler<ChangePersonalInfoCommand, List<UserRole>>
    {
        private readonly IUserRepository _userRepository;

        public ChangePersonalInfoCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserRole>> Handle(ChangePersonalInfoCommand request, CancellationToken cancellationToken)
        {
            User user = await _userRepository.GetByUsernameAsync(request.UserName);

            if (request.NewFirstName != null)
            {
                user.ChangeFirstName(request.NewFirstName);
            }
            if (request.NewLastName != null)
            {
                user.ChangeLastName(request.NewLastName);
            }
            if (request.NewUsername != null || request.NewUsername != user.UserName)
            {
                User existingReaderWithNewUsername = await _userRepository.GetByUsernameAsync(request.NewUsername);
                if (existingReaderWithNewUsername == null)
                {
                    user.ChangeUsername(request.NewUsername);
                }
            }

            _userRepository.UpdateUser(user);
            return user.Roles;
        }
    }
}
