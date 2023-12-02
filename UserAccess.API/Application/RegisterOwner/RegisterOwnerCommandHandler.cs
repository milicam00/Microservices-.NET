using BuildingBlocks.Domain;
using UserAccess.Domain.Users;
using UserAccess.Infrastructure.Configuration.Commands;
using UserAccess.Infrastructure.Domain.Users;

namespace UserAccess.API.Application.RegisterOwner
{
    public class RegisterOwnerCommandHandler : ICommandHandler<RegisterOwnerCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        public RegisterOwnerCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Result> Handle(RegisterOwnerCommand request, CancellationToken cancellationToken)
        {
            var password = PasswordManager.HashPassword(request.Password);
            User userWithSameUsername = await _userRepository.GetByUsernameAsync(request.Username);
            if (userWithSameUsername != null)
            {
                return Result.Failure("User with same username already exist.");
            }
            var user = User.CreateOwner(
                request.Username,
                password,
                request.Email,
                request.FirstName,
                request.LastName
                );
            await _userRepository.AddAsync(user);

            var userDto = new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive

            };

            return Result.Success(userDto);

        }
    }
}
