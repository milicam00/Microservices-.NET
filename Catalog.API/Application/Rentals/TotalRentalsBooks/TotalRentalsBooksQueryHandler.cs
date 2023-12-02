using BuildingBlocks.Application.Data;
using BuildingBlocks.Domain;
using Catalog.Infrastructure.Configuration.Queries;
using Dapper;

namespace Catalog.API.Application.Rentals.TotalRentalsBooks
{
    public class TotalRentalBooksQueryHandler : IQueryHandler<TotalRentalBooksQuery, Result>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public TotalRentalBooksQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result> Handle(TotalRentalBooksQuery request, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();
            var parameters = new { StartDate = request.StartDate, EndDate = request.EndDate };

            string sql = "SELECT COUNT(R.[BookId]) AS TotalBooks " +
             "FROM [RentalBooks] AS [R] " +
             "LEFT JOIN [Rentals] AS [A] ON R.[RentalId] = A.[RentalId] " +
             "WHERE [A].[RentalDate] BETWEEN @StartDate AND @EndDate";
            var result = (await connection.QueryAsync<int>(sql, parameters));
            return Result.Success(result);
        }
    }
}
