using BuildingBlocks.Domain;
using UserAccess.Infrastructure.Contracts;

namespace UserAccess.API.Application.UploadProfileImage
{
    public class UploadProfileImageCommand : CommandBase<Result>
    {
        public UploadProfileImageCommand(IFormFile imageFile, string wwwRootPath, string folder, string username)
        {
            ImageFile = imageFile;
            WwwRootPath = wwwRootPath;
            Folder = folder;
            UserName = username;
        }
        public IFormFile ImageFile { get; set; }
        public string WwwRootPath { get; set; }
        public string Folder { get; set; }
        public string UserName { get; set; }
    }
}
