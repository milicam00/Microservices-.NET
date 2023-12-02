using BuildingBlocks.Application.Data;
using BuildingBlocks.Domain;
using Catalog.Domain.Owners;
using Catalog.Infrastructure.Configuration.Queries;
using Dapper;

namespace Catalog.API.Application.Rentals.TopTenMostPopularOwners
{
    public class TopTenMostPopularOwnersQueryHandler : IQueryHandler<TopTenMostPopularOwnersQuery, Result>
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        public TopTenMostPopularOwnersQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<Result> Handle(TopTenMostPopularOwnersQuery request, CancellationToken cancellationToken)
        {
            var connection = _connectionFactory.GetOpenConnection();
            var parameters = new { StartDate = request.StartDate, EndDate = request.EndDate };

            string sql = @"
                SELECT TOP 10
                O.[OwnerId] AS [OwnerId],
                O.[UserName] AS [UserName],
                O.[Email] AS [Email],
                O.[FirstName] AS [FirstName],
                O.[LastName] AS [LastName],
                L.[LibraryName] AS [LibraryName]
                FROM [Owners] AS [O]
                JOIN [Libraries] AS [L] ON O.[OwnerId] = L.[OwnerId]
                JOIN [Books] AS [B] ON L.[LibraryId] = B.[LibraryId]
                JOIN [RentalBooks] AS [RB] ON B.[BookId] = RB.[BookId]
                JOIN [Rentals] AS [R] ON RB.[RentalId] = R.[RentalId]
                WHERE R.[RentalDate] BETWEEN @StartDate AND @EndDate
                GROUP BY O.[OwnerId], O.[UserName], O.[Email],O.[FirstName],O.[LastName],L.[LibraryName]
                ORDER BY COUNT(*) DESC;
                ";


            var result = (await connection.QueryAsync<OwnerDto>(sql, parameters)).AsList();
            return Result.Success(result);

        }
    }
}
