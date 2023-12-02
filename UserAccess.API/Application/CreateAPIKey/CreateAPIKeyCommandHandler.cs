using BuildingBlocks.Domain;
using UserAccess.Domain.APIKeys;
using UserAccess.Domain.Users;
using UserAccess.Infrastructure.Configuration.Commands;

namespace UserAccess.API.Application.CreateAPIKey
{
    public class CreateAPIKeyCommandHandler : ICommandHandler<CreateAPIKeyCommand, Result>
    {
        private readonly IAPIKeyRepository _apiKeyRepository;
        private readonly IUserRepository _userRepository;
        public CreateAPIKeyCommandHandler(IAPIKeyRepository apiKeyRepository, IUserRepository userRepository)
        {
            _apiKeyRepository = apiKeyRepository;
            _userRepository = userRepository;
        }
        public async Task<Result> Handle(CreateAPIKeyCommand request, CancellationToken cancellationToken)
        {
            User user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null)
            {
                return Result.Failure("This user does not exist.");
            }

            var apiKey = APIKey.CreateKey(request.Username, request.KeyName);
            await _apiKeyRepository.AddAsync(apiKey);

            return Result.Success(apiKey);
        }
    }
}
