using BuildingBlocks.Domain;
using UserAccess.Domain.APIKeys;
using UserAccess.Infrastructure.Configuration.Commands;

namespace UserAccess.API.Application.EditAPIKey
{
    public class EditAPIKeyCommandHandler : ICommandHandler<EditAPIKeyCommand, Result>
    {
        private readonly IAPIKeyRepository _apiKeyRepository;
        public EditAPIKeyCommandHandler(IAPIKeyRepository apiKeyRepository)
        {
            _apiKeyRepository = apiKeyRepository;
        }
        public async Task<Result> Handle(EditAPIKeyCommand request, CancellationToken cancellationToken)
        {
            var apiKey = await _apiKeyRepository.GetAsync(request.Username);
            if (apiKey == null)
            {
                return Result.Failure("Invalid API key.");
            }

            apiKey.Name = request.NewName;
            _apiKeyRepository.Update(apiKey);
            return Result.Success(apiKey);
        }
    }
}
