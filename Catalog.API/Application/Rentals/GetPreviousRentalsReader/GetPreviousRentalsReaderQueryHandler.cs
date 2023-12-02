using BuildingBlocks.Application.Data;
using BuildingBlocks.Domain;
using Catalog.Domain.Readers;
using Catalog.Domain.Rentals;
using Catalog.Infrastructure.Configuration.Queries;
using Dapper;
using System.Data;

namespace Catalog.API.Application.Rentals.GetPreviousRentalsReader
{
    public class GetPreviousRentalsReaderQueryHandler : IQueryHandler<GetPreviousRentalsReaderQuery, Result>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly IReaderRepository _readerRepository;

        public GetPreviousRentalsReaderQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IReaderRepository readerRepository)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _readerRepository = readerRepository;
        }

        public async Task<Result> Handle(GetPreviousRentalsReaderQuery request, CancellationToken cancellationToken)
        {
            var reader = await _readerRepository.GetByUsername(request.ReaderUsername);
            if (reader == null)
            {
                return Result.Failure("This reader does not exist");
            }

            var connection = _sqlConnectionFactory.GetOpenConnection();


            var parameters = new DynamicParameters();
            parameters.Add("ReaderId", reader.ReaderId);

            string sql = "SELECT " +
                         $"R.[RentalId] AS [{nameof(PreviousRentalDto.RentalId)}], " +
                         $"R.[RentalDate] AS [{nameof(PreviousRentalDto.RentalDate)}], " +
                         $"R.[ReturnDate] AS [{nameof(PreviousRentalDto.ReturnDate)}], " +
                         $"R.[Returned] AS [{nameof(PreviousRentalDto.Returned)}], " +
                         $"RB.[BookId] AS [{nameof(PreviousRentalDto.BookId)}], " +
                         $"B.[Title] AS [{nameof(PreviousRentalDto.Title)}] " +
                         "FROM [Rentals] AS [R] " +
                         "LEFT JOIN [RentalBooks] AS [RB] ON R.[RentalId] = RB.[RentalId] " +
                         "LEFT JOIN [Books] AS [B] ON RB.[BookId] = B.[BookId] " +
                         "WHERE [R].[ReaderId] = @ReaderId ";

            if (!string.IsNullOrEmpty(request.Title))
            {
                var lowerQueryTitle = request.Title.ToLower();
                sql += "AND LOWER(B.[Title]) LIKE @Title + '%' ";
                parameters.Add("Title", "%" + lowerQueryTitle + "%");
            }

            if (request.OrderBy != null)
            {
                sql += "ORDER BY " + GetOrderBy(request.OrderBy);
            }
            else
            {
                sql += "ORDER BY RB.[BookId]";
            }

            sql += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            parameters.Add("Offset", (request.PageNumber - 1) * request.PageSize);
            parameters.Add("PageSize", request.PageSize);
            parameters.Add("OrderBy", request.OrderBy);

            var result = (await connection.QueryAsync<PreviousRentalDto>(sql, parameters)).AsList();

            var groupedResult = result.GroupBy(r => r.RentalId).Select(group => new PreviousRentalsResult
            {
                RentalId = group.Key,
                RentalDate = group.First().RentalDate,
                ReturnDate = group.First().ReturnDate,
                IsReturned = group.First().Returned,
                Books = group.Select(book => new PreviousRentalBookDto
                {
                    BookId = book.BookId,
                    Title = book.Title
                }).ToList()
            }).ToList();

            var totalRecords = await GetTotalRecordsAsync(connection, parameters);
            var paginatedResult = new PaginationResult<PreviousRentalsResult>(request.PageNumber, request.PageSize, totalRecords, groupedResult);

            return Result.Success(paginatedResult);
        }

        private async Task<int> GetTotalRecordsAsync(IDbConnection connection, DynamicParameters parameters)
        {
            string countSql = "SELECT COUNT(DISTINCT R.[RentalId]) " +
                              "FROM [Rentals] AS [R] " +
                              "LEFT JOIN [RentalBooks] AS [RB] ON R.[RentalId] = RB.[RentalId] " +
                              "LEFT JOIN [Books] AS [B] ON RB.[BookId] = B.[BookId] " +
                              "WHERE [R].[ReaderId] = @ReaderId ";

            if (parameters.ParameterNames.Any(paramName => paramName == "Title"))
            {
                countSql += "AND LOWER(B.[Title]) LIKE @Title + '%' ";
            }

            return await connection.ExecuteScalarAsync<int>(countSql, parameters);
        }

        private string GetOrderBy(string orderBy)
        {
            var orderByClauses = orderBy.Split(',');
            var sqlOrderByClauses = new List<string>();
            foreach (var clause in orderByClauses)
            {
                switch (clause.Trim())
                {
                    case "Title":
                        sqlOrderByClauses.Add("B.[Title]");
                        break;
                    case "-Title":
                        sqlOrderByClauses.Add("B.[Title] DESC");
                        break;
                    case "RentalDate":
                        sqlOrderByClauses.Add("R.[RentalDate]");
                        break;
                    case "-RentalDate":
                        sqlOrderByClauses.Add("R.[RentalDate] DESC");
                        break;

                    default:
                        sqlOrderByClauses.Add("RB.[BookId]");
                        break;
                }
            }
            if (sqlOrderByClauses.Count == 0)
            {
                sqlOrderByClauses.Add("R.[RentalDate]");
            }
            return string.Join(", ", sqlOrderByClauses);
        }
    }
}
