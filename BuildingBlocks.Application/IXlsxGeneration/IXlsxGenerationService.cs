using BuildingBlocks.Domain.Results;

namespace BuildingBlocks.Application.IXlsxGeneration
{
    public interface IXlsxGenerationService
    {
        byte[] SerializeBooksToXlsx(List<BookOfOwnerDto> books);
        Task<List<ImportedBookDto>> DeserializeBooksFromXlsx(Stream fileStream);
        Task<List<ImportedBookForOneLibraryDto>> DeserializeBooksForOneLibraryFromXlsx(Stream stream);
    }
}
