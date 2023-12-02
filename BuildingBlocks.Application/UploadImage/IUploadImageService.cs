using BuildingBlocks.Domain.Results;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Application.UploadImage
{
    public interface IUploadImageService
    {
        Image UploadImage(IFormFile imageFile, string wwwRootPath, string folder);
    }
}
