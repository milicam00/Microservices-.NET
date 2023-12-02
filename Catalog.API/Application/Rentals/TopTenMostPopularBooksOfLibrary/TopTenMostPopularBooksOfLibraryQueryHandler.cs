using BuildingBlocks.Application.Data;
using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Domain.Libraries;
using Catalog.Infrastructure.Configuration.Queries;
using Dapper;

namespace Catalog.API.Application.Rentals.TopTenMostPopularBooksOfLibrary
{
    public class TopTenMostPopularBooksOfLibraryQueryHandler : IQueryHandler<TopTenMostPopularBooksOfLibraryQuery, Result>
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly ILibraryRepository _libraryRepository;
        public TopTenMostPopularBooksOfLibraryQueryHandler(ISqlConnectionFactory connectionFactory, ILibraryRepository libraryRepository)
        {
            _connectionFactory = connectionFactory;
            _libraryRepository = libraryRepository;
        }
        public async Task<Result> Handle(TopTenMostPopularBooksOfLibraryQuery request, CancellationToken cancellationToken)
        {
            var connection = _connectionFactory.GetOpenConnection();
            var parameters = new { LibraryId = request.LibraryId, StartDate = request.StartDate, EndDate = request.EndDate };

            var library = await _libraryRepository.GetByIdAsync(request.LibraryId);
            if (library == null)
            {
                return Result.Failure("This library does not exist.");
            }
            string sql = @"
            SELECT TOP 10
            A.[BookId] AS [BookId], 
            A.[Title] AS [Title], 
            A.[Description] AS [Description], 
            A.[Author] AS [Author], 
            A.[Pages] AS [Pages], 
            A.[Genres] AS [Genres], 
            A.[PubblicationYear] AS [PubblicationYear], 
            A.[UserRating] AS [UserRating], 
            A.[NumberOfCopies] AS [NumberOfCopies]
            FROM [Books] AS [A]
            WHERE A.[BookId] IN (
                SELECT TOP 10 B.[BookId]
                FROM [Libraries] AS [L]
                JOIN [Books] AS [B] ON L.[LibraryId] = B.[LibraryId]
                JOIN [RentalBooks] AS [RB] ON B.[BookId] = RB.[BookId]
                JOIN [Rentals] AS [R] ON RB.[RentalId] = R.[RentalId]
                WHERE R.[RentalDate] BETWEEN @StartDate AND @EndDate AND L.[LibraryId] = @LibraryId
                GROUP BY B.[BookId]
                ORDER BY COUNT(*) DESC
            );";

            var result = (await connection.QueryAsync<BookDto>(sql, parameters)).AsList();
            return Result.Success(result);
        }
    }
}
