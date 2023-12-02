using BuildingBlocks.Application.Data;
using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Infrastructure.Configuration.Queries;
using Dapper;

namespace Catalog.API.Application.Rentals.TopTenMostPopularBooks
{
    public class TopTenMostPopularBooksQueryHandler : IQueryHandler<TopTenMostPopularBooksQuery, Result>
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        public TopTenMostPopularBooksQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Result> Handle(TopTenMostPopularBooksQuery request, CancellationToken cancellationToken)
        {
            var connection = _connectionFactory.GetOpenConnection();
            var parameters = new { StartDate = request.StartDate, EndDate = request.EndDate };

            string sql = @"             
                SELECT TOP 10
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
                WHERE B.[BookId] IN (
                SELECT TOP 10 R.[BookId]
                FROM [RentalBooks] AS [R]
                JOIN [Rentals] AS [A] ON R.[RentalId] = A.[RentalId]
                WHERE A.[RentalDate] BETWEEN @StartDate AND @EndDate
                GROUP BY R.[BookId]
                ORDER BY COUNT(*) DESC
                );";

            var result = (await connection.QueryAsync<BookDto>(sql, parameters)).AsList();
            return Result.Success(result);
        }
    }
}
