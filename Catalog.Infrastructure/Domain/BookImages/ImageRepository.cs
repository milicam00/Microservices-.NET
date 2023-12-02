using Catalog.Domain.BookImages;

namespace Catalog.Infrastructure.Domain.BookImages
{
    public class ImageRepository : IImageRepository
    {
        private readonly CatalogContext _catalogContext;
        public ImageRepository(CatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }
        public async Task AddAsync(Image image)
        {
            await _catalogContext.AddAsync(image);
        }
    }
}
