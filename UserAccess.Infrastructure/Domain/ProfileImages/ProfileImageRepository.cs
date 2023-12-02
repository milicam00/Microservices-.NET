using UserAccess.Domain.ProfileImages;

namespace UserAccess.Infrastructure.Domain.ProfileImages
{
    public class ProfileImageRepository : IProfileImageRepository
    {
        private readonly UserAccessContext _userAccessContext;
        public ProfileImageRepository(UserAccessContext userAccessContext)
        {
            _userAccessContext = userAccessContext;
        }
        public async Task AddAsync(ProfileImage image)
        {
            await _userAccessContext.AddAsync(image);
        }
    }
}
