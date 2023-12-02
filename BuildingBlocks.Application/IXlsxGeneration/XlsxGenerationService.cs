using BuildingBlocks.Domain.Results;
using OfficeOpenXml;

namespace BuildingBlocks.Application.IXlsxGeneration
{
    public class XlsxGenerationService : IXlsxGenerationService
    {
        public byte[] SerializeBooksToXlsx(List<BookOfOwnerDto> books)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Books");
                worksheet.Cells.LoadFromCollection(books, true);
                return package.GetAsByteArray();
            }
        }

        public async Task<List<ImportedBookDto>> DeserializeBooksFromXlsx(Stream fileStream)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(fileStream))
            {
                var worksheet = package.Workbook.Worksheets.First();
                var rowCount = worksheet.Dimension.Rows;
                var books = new List<ImportedBookDto>();

                for (int row = 2; row <= rowCount; row++)
                {
                    var book = new ImportedBookDto
                    {
                        Title = worksheet.Cells[row, 2].GetCellValue<string>(),
                        Description = worksheet.Cells[row, 3].GetCellValue<string>(),
                        Author = worksheet.Cells[row, 4].GetCellValue<string>(),
                        Pages = worksheet.Cells[row, 5].GetCellValue<int>(),
                        Genres = worksheet.Cells[row, 6].GetCellValue<int>(),
                        PubblicationYear = worksheet.Cells[row, 7].GetCellValue<int>(),
                        UserRating = worksheet.Cells[row, 8].GetCellValue<double>(),
                        NumberOfCopies = worksheet.Cells[row, 9].GetCellValue<int>(),
                        NumberOfRatings = worksheet.Cells[row, 10].GetCellValue<int>(),
                        IsDeleted = worksheet.Cells[row, 11].GetCellValue<bool>(),
                        LibraryId = worksheet.Cells[row, 12].GetCellValue<string>()
                    };
                    books.Add(book);
                }

                return books;
            }
        }

        public async Task<List<ImportedBookForOneLibraryDto>> DeserializeBooksForOneLibraryFromXlsx(Stream stream)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.First();
                var rowCount = worksheet.Dimension.Rows;
                var books = new List<ImportedBookForOneLibraryDto>();

                for (int row = 2; row <= rowCount; row++)
                {
                    var book = new ImportedBookForOneLibraryDto
                    {
                        Title = worksheet.Cells[row, 2].GetCellValue<string>(),
                        Description = worksheet.Cells[row, 3].GetCellValue<string>(),
                        Author = worksheet.Cells[row, 4].GetCellValue<string>(),
                        Pages = worksheet.Cells[row, 5].GetCellValue<int>(),
                        Genres = worksheet.Cells[row, 6].GetCellValue<int>(),
                        PubblicationYear = worksheet.Cells[row, 7].GetCellValue<int>(),
                        UserRating = worksheet.Cells[row, 8].GetCellValue<double>(),
                        NumberOfCopies = worksheet.Cells[row, 9].GetCellValue<int>(),
                        NumberOfRatings = worksheet.Cells[row, 10].GetCellValue<int>(),
                        IsDeleted = worksheet.Cells[row, 11].GetCellValue<bool>()

                    };
                    books.Add(book);
                }

                return books;
            }
        }
    }
}
