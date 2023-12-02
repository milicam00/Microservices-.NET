using BuildingBlocks.Application.UploadImage;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.Results;
using UserAccess.Domain.ProfileImages;
using UserAccess.Domain.Users;
using UserAccess.Infrastructure.Configuration.Commands;

namespace UserAccess.API.Application.UploadProfileImage
{
    public class UploadProfileImageCommandHandler : ICommandHandler<UploadProfileImageCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUploadImageService _uploadImageService;
        private readonly IProfileImageRepository _profileImageRepository;
        public UploadProfileImageCommandHandler(IUserRepository userRepository, IUploadImageService uploadImageService, IProfileImageRepository profileImageRepository)
        {
            _userRepository = userRepository;
            _uploadImageService = uploadImageService;
            _profileImageRepository = profileImageRepository;
        }
        public async Task<Result> Handle(UploadProfileImageCommand request, CancellationToken cancellationToken)
        {
            User user = await _userRepository.GetByUsernameAsync(request.UserName);
            if (user == null)
            {
                return Result.Failure("This user does not exist.");
            }

            Image image = _uploadImageService.UploadImage(request.ImageFile, request.WwwRootPath, request.Folder);
            ProfileImage profileImage = (ProfileImage)image;

            user.ProfileImage = profileImage;
            user.ProfileImageId = profileImage.ProfileImageId;
            _userRepository.UpdateUser(user);
            await _profileImageRepository.AddAsync(profileImage);

            return Result.Success(user);
        }
    }
}
