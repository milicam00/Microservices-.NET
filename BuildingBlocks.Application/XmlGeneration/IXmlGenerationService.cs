using BuildingBlocks.Domain.Results;

namespace BuildingBlocks.Application.XmlGeneration
{
    public interface IXmlGenerationService
    {
        string SerializeBooksToXml(List<BookOfOwnerDto> books);
        List<BookOfOwnerDto> DeserializeBooksFromXml(Stream fileStream);
        List<ImportedBookForOneLibraryDto> DeserializeBooksForOneLibraryFromXml(Stream fileStream);
    }
}
