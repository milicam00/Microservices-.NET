using BuildingBlocks.Domain.Results;
using System.Xml.Serialization;
using System.Xml;

namespace BuildingBlocks.Application.XmlGeneration
{
    public class XmlGenerationService : IXmlGenerationService
    {
        public string SerializeBooksToXml(List<BookOfOwnerDto> books)
        {
            var serializer = new XmlSerializer(typeof(List<BookOfOwnerDto>));
            var stringWriter = new StringWriter();
            using (XmlWriter writer = new XmlTextWriter(stringWriter))
            {
                serializer.Serialize(writer, books);
                return stringWriter.ToString();
            }
        }

        public List<ImportedBookForOneLibraryDto> DeserializeBooksForOneLibraryFromXml(Stream fileStream)
        {
            List<ImportedBookForOneLibraryDto> books;
            using (XmlReader xmlReader = XmlReader.Create(fileStream))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<BookOfOwnerDto>));
                books = (List<ImportedBookForOneLibraryDto>)serializer.Deserialize(xmlReader);
            }
            return books;
        }

        public List<BookOfOwnerDto> DeserializeBooksFromXml(Stream fileStream)
        {
            List<BookOfOwnerDto> books;
            using (XmlReader xmlReader = XmlReader.Create(fileStream))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<BookOfOwnerDto>));
                books = (List<BookOfOwnerDto>)serializer.Deserialize(xmlReader);
            }
            return books;
        }


    }
}
