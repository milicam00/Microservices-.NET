using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Books.UploadBookImage
{
    public class UploadBookImageCommand : CommandBase<Result>
    {
        public UploadBookImageCommand(IFormFile imageFile, string wwwRootPath, string folder, Guid bookId, string username)
        {
            ImageFile = imageFile;
            WwwRootPath = wwwRootPath;
            Folder = folder;
            BookId = bookId;
            Username = username;
        }
        public IFormFile ImageFile { get; set; }
        public string WwwRootPath { get; set; }
        public string Folder { get; set; }
        public Guid BookId { get; set; }
        public string Username { get; set; }
    }
}
