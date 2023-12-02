using BuildingBlocks.Application.Data;
using BuildingBlocks.Domain;
using Catalog.Domain.Libraries;
using Catalog.Infrastructure.Configuration.Queries;
using Dapper;

namespace Catalog.API.Application.Rentals.TotalRentalBooksOfLibrary
{
    public class TotalRentalBooksOfLibraryQueryHandler : IQueryHandler<TotalRentalBooksOfLibraryQuery, Result>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly ILibraryRepository _libraryRepository;
        public TotalRentalBooksOfLibraryQueryHandler(ISqlConnectionFactory sqlConnectionFactory, ILibraryRepository libraryRepository)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _libraryRepository = libraryRepository;
        }

        public async Task<Result> Handle(TotalRentalBooksOfLibraryQuery request, CancellationToken cancellationToken)
        {
            var library = await _libraryRepository.GetByIdAsync(request.LibraryId);
            if (library == null)
            {
                return Result.Failure("This library does not exist.");
            }
            var connection = _sqlConnectionFactory.GetOpenConnection();
            var parameters = new { LibraryId = request.LibraryId, StartDate = request.StartDate, EndDate = request.EndDate };

            string sql = "SELECT COUNT(R.[BookId]) AS TotalBooks " +
             "FROM [Libraries] AS [L] " +
             "LEFT JOIN [Books] AS [B] ON L.[LibraryId] = B.[LibraryId] " +
             "LEFT JOIN [RentalBooks] AS [R] ON B.[BookId] = R.[BookId] " +
             "LEFT JOIN [Rentals] AS [A] ON R.[RentalId] = A.[RentalId] " +
             "WHERE [L].[LibraryId] = @LibraryId " +
             "AND [A].[RentalDate] BETWEEN @StartDate AND @EndDate";
            var result = (await connection.QueryAsync<int>(sql, parameters));
            return Result.Success(result);

        }
    }
}
