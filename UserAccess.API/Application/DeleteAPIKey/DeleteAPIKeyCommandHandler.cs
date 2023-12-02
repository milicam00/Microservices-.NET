using BuildingBlocks.Domain;
using UserAccess.Domain.APIKeys;
using UserAccess.Infrastructure.Configuration.Commands;

namespace UserAccess.API.Application.DeleteAPIKey
{
    public class DeleteApiKeyCommandHandler : ICommandHandler<DeleteApiKeyCommand, Result>
    {
        private readonly IAPIKeyRepository _apiKeyRepository;
        public DeleteApiKeyCommandHandler(IAPIKeyRepository apiKeyRepository)
        {
            _apiKeyRepository = apiKeyRepository;
        }
        public async Task<Result> Handle(DeleteApiKeyCommand request, CancellationToken cancellationToken)
        {
            APIKey? apiKey = await _apiKeyRepository.GetAsync(request.Username);
            if (apiKey == null)
            {
                return Result.Failure("This user has no key.");
            }

            _apiKeyRepository.DeleteApiKey(apiKey);
            return Result.Success(apiKey);
        }
    }
}
