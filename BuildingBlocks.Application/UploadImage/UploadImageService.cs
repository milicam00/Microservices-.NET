using BuildingBlocks.Domain.Results;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Application.UploadImage
{
    public class UploadImageService : IUploadImageService
    {

        public UploadImageService()
        {

        }

        public Image UploadImage(IFormFile imageFile, string wwwRootPath, string folder)
        {
            try
            {
                if (imageFile == null || imageFile.Length == 0)
                {
                    throw new Exception("Image does not exist.");
                }

                using (var ms = new MemoryStream())
                {
                    imageFile.CopyTo(ms);
                    byte[] imageBytes = ms.ToArray();
                    string uniqueImageName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);


                    string destinationPath = Path.Combine(wwwRootPath, folder, uniqueImageName);


                    File.WriteAllBytes(destinationPath, imageBytes);
                    using (var image = System.Drawing.Image.FromStream(new MemoryStream(imageBytes)))
                    {
                        int width = image.Width;
                        int height = image.Height;
                        Image dbImage = new Image
                        {
                            Path = $"/{folder}/{uniqueImageName}",
                            Dimensions = $"{width}x{height}",
                            Format = Path.GetExtension(imageFile.FileName).TrimStart('.'),
                        };

                        return dbImage;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }
    }
}
