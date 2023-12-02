namespace UserAccess.Domain.ProfileImages
{
    public interface IProfileImageRepository
    {
        Task AddAsync(ProfileImage image);
    }
}
