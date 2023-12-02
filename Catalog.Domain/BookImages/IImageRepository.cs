namespace Catalog.Domain.BookImages
{
    public interface IImageRepository
    {
        Task AddAsync(Image image);
    }
}
