using BuildingBlocks.Application.Data;
using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Domain.Libraries;
using Catalog.Infrastructure.Configuration.Queries;
using Dapper;

namespace Catalog.API.Application.Rentals.TopFiveRatedBooksOfLibrary
{
    public class TopFiveRatedBooksOfLibraryQueryHandler : IQueryHandler<TopFiveRatedBooksOfLibraryQuery, Result>
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly ILibraryRepository _libraryRepository;
        public TopFiveRatedBooksOfLibraryQueryHandler(ISqlConnectionFactory connectionFactory, ILibraryRepository libraryRepository)
        {
            _connectionFactory = connectionFactory;
            _libraryRepository = libraryRepository;
        }
        public async Task<Result> Handle(TopFiveRatedBooksOfLibraryQuery request, CancellationToken cancellationToken)
        {
            var connection = _connectionFactory.GetOpenConnection();
            var parameters = new { LibraryId = request.LibraryId, StartDate = request.StartDate, EndDate = request.EndDate };

            var library = await _libraryRepository.GetByIdAsync(request.LibraryId);
            if (library == null)
            {
                return Result.Failure("This library does not exist.");
            }
            string sql = @"
            SELECT TOP 5
                B.[BookId] AS [BookId], 
                B.[Title] AS [Title], 
                B.[Description] AS [Description], 
                B.[Author] AS [Author], 
                B.[Pages] AS [Pages], 
                B.[Genres] AS [Genres], 
                B.[PubblicationYear] AS [PubblicationYear], 
                B.[UserRating] AS [UserRating], 
                B.[NumberOfCopies] AS [NumberOfCopies]
                FROM [Books] AS [B]
                WHERE B.[UserRating] IS NOT NULL
                AND B.[BookId] IN (
                SELECT TOP 5 B.[BookId]
                FROM [Books] AS [B]
                LEFT JOIN [RentalBooks] AS [R] ON B.[BookId] = R.[BookId]
                LEFT JOIN [Rentals] AS [A] ON R.[RentalId] = A.[RentalId]
                WHERE B.[UserRating] IS NOT NULL AND B.[LibraryId] = @LibraryId AND [A].[RentalDate] BETWEEN @StartDate AND @EndDate
                GROUP BY B.[BookId]
                ORDER BY AVG(B.[UserRating]) DESC
            );";

            var result = (await connection.QueryAsync<BookDto>(sql, parameters)).AsList();
            return Result.Success(result);
        }
    }
}
