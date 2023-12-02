using BuildingBlocks.Application.Data;
using BuildingBlocks.Domain;
using Catalog.Domain.Owners;
using Catalog.Domain.Rentals;
using Catalog.Infrastructure.Configuration.Queries;
using Dapper;

namespace Catalog.API.Application.Rentals.GetUnreturnedBooks
{
    public class GetUnreturnedBooksQueryHandler : IQueryHandler<GetUnreturnedBooksQuery, Result>
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly IOwnerRepository _ownerRepository;

        public GetUnreturnedBooksQueryHandler(ISqlConnectionFactory connectionFactory, IOwnerRepository ownerRepository)
        {
            _connectionFactory = connectionFactory;
            _ownerRepository = ownerRepository;
        }

        public async Task<Result> Handle(GetUnreturnedBooksQuery request, CancellationToken cancellationToken)
        {
            var owner = await _ownerRepository.GetByUsername(request.OwnerUsername);
            if (owner == null)
            {
                return Result.Failure("This owner does not exist");
            }

            var connection = _connectionFactory.GetOpenConnection();

            var parameters = new DynamicParameters();
            parameters.Add("OwnerId", owner.OwnerId);

            string sql = "SELECT " +
              $"L.[LibraryName] AS [{nameof(UnreturnedBooksDto.LibraryName)}], " +
              $"B.[BookId] AS [{nameof(UnreturnedBooksDto.BookId)}], " +
              $"B.[Title] AS [{nameof(UnreturnedBooksDto.BookTitle)}], " +
              $"U.[UserName] AS [{nameof(UnreturnedBooksDto.UserName)}], " +
              $"R.[RentalDate] AS [{nameof(UnreturnedBooksDto.RentalDate)}], " +
              $"R.[Returned] AS [{nameof(UnreturnedBooksDto.Returned)}] " +
              "FROM [Libraries] AS [L] " +
              "LEFT JOIN [Books] AS [B] ON L.[LibraryId] = B.[LibraryId] " +
              "LEFT JOIN [RentalBooks] AS [RB] ON B.[BookId] = RB.[BookId] " +
              "LEFT JOIN [Rentals] AS [R] ON RB.[RentalId] = R.[RentalId] " +
              "LEFT JOIN [Readers] AS [U] ON R.[ReaderId] = U.[ReaderId] " +
              "WHERE [L].[OwnerId] = @OwnerId " +
              "AND R.[RentalDate] <= DATEADD(DAY, -30, GETDATE()) " +
              "AND R.[Returned] = 0 ";

            if (!string.IsNullOrEmpty(request.LibraryName))
            {
                var lowerQueryTitle = request.LibraryName.ToLower();
                sql += "AND LOWER(L.[LibraryName]) LIKE @LibraryName + '%' ";
                parameters.Add("LibraryName", "%" + lowerQueryTitle + "%");
            }
            if (!string.IsNullOrEmpty(request.BookTitle))
            {
                var lowerQueryTitle = request.BookTitle.ToLower();
                sql += "AND LOWER(B.[Title]) LIKE @BookTitle + '%' ";
                parameters.Add("BookTitle", "%" + lowerQueryTitle + "%");
            }

            if (request.OrderBy != null)
            {
                sql += "ORDER BY " + GetOrderBy(request.OrderBy);
            }
            else
            {
                sql += "ORDER BY R.[RentalDate]";
            }

            sql += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            parameters.Add("Offset", (request.PageNumber - 1) * request.PageSize);
            parameters.Add("PageSize", request.PageSize);
            parameters.Add("OrderBy", request.OrderBy);
            var result = (await connection.QueryAsync<UnreturnedBooksDto>(sql, parameters)).AsList();
            var totalRecords = result.Count();
            var paginatedResult = new PaginationResult<UnreturnedBooksDto>(request.PageNumber, request.PageSize, totalRecords, result);

            return Result.Success(paginatedResult);
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
                    case "UserName":
                        sqlOrderByClauses.Add("U.[UserName]");
                        break;
                    case "-UserName":
                        sqlOrderByClauses.Add("U.[UserName] DESC");
                        break;
                    case "RentalDate":
                        sqlOrderByClauses.Add("R.[RentalDate]");
                        break;
                    case "-RentalDate":
                        sqlOrderByClauses.Add("R.[RentalDate] DESC");
                        break;
                    case "LibraryName":
                        sqlOrderByClauses.Add("L.[LibraryName]");
                        break;
                    case "-LibraryName":
                        sqlOrderByClauses.Add("L.[LibraryName] DESC");
                        break;
                    default:
                        sqlOrderByClauses.Add("B.[BookId]");
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
