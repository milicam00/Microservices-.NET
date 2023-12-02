using BuildingBlocks.Domain.Results;
using CsvHelper;
using System.Globalization;

namespace BuildingBlocks.Application.ICsvGeneration
{
    public class CsvGenerationService : ICsvGenerationService
    {
        public byte[] SerializeBooksToCsv(List<BookOfOwnerDto> books)
        {
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
            {
                csvWriter.WriteRecords(books);
                streamWriter.Flush();
                return memoryStream.ToArray();
            }
        }

        public async Task<List<BookDto>> DeserializeBooksFromCsv(Stream fileStream)
        {
            using (var reader = new StreamReader(fileStream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                List<BookDto> books = csv.GetRecords<BookDto>().ToList();
                return books;
            }
        }

        public async Task<List<ImportedBookForOneLibraryDto>> DeserializeBooksFromCsvForOneLibrary(Stream fileStream)
        {
            using (var reader = new StreamReader(fileStream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                List<ImportedBookForOneLibraryDto> books = csv.GetRecords<ImportedBookForOneLibraryDto>().ToList();
                return books;
            }
        }
    }
}
