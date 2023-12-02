using BuildingBlocks.Application.Data;
using BuildingBlocks.Domain;
using Catalog.Domain.Owners;
using Catalog.Domain.Rentals;
using Catalog.Infrastructure.Configuration.Queries;
using Dapper;

namespace Catalog.API.Application.Rentals.GetPreviousRentalsOwner
{
    public class GetPreviousRentalsOwnerQueryHandler : IQueryHandler<GetPreviousRentalsOwnerQuery, Result>
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly IOwnerRepository _ownerRepository;
        public GetPreviousRentalsOwnerQueryHandler(ISqlConnectionFactory connectionFactory, IOwnerRepository ownerRepository)
        {
            _connectionFactory = connectionFactory;
            _ownerRepository = ownerRepository;
        }

        public async Task<Result> Handle(GetPreviousRentalsOwnerQuery request, CancellationToken cancellationToken)
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
             $"L.[LibraryName] AS [{nameof(PreviousRentalOwnerDto.LibraryName)}], " +
             $"B.[Title] AS [{nameof(PreviousRentalOwnerDto.BookName)}], " +
             $"R.[RentalId] AS [{nameof(PreviousRentalOwnerDto.RentalId)}], " +
             $"A.[RentalDate] AS [{nameof(PreviousRentalOwnerDto.RentalDate)}], " +
             $"A.[Returned] AS [{nameof(PreviousRentalOwnerDto.Returned)}], " +
             $"U.[UserName] AS [{nameof(PreviousRentalOwnerDto.UserName)}] " +
             "FROM [Libraries] AS [L] " +
             "LEFT JOIN [Books] AS [B] ON L.[LibraryId] = B.[LibraryId] " +
             "LEFT JOIN [RentalBooks] AS [R] ON B.[BookId] = R.[BookId] " +
             "LEFT JOIN [Rentals] AS [A] ON R.[RentalId] = A.[RentalId] " +
             "LEFT JOIN [Readers] AS [U] ON A.[ReaderId] = U.[ReaderId] " +
             "WHERE [L].[OwnerId] = @OwnerId ";

            if (!string.IsNullOrEmpty(request.LibraryName))
            {
                var lowerQueryTitle = request.LibraryName.ToLower();
                sql += "AND LOWER(L.[LibraryName]) LIKE @LibraryName + '%' ";
                parameters.Add("LibraryName", "%" + lowerQueryTitle + "%");
            }
            if (!string.IsNullOrEmpty(request.UserName))
            {
                var lowerQueryTitle = request.UserName.ToLower();
                sql += "AND LOWER(U.[UserName]) LIKE @UserName + '%' ";
                parameters.Add("UserName", "%" + lowerQueryTitle + "%");
            }
            if (request.IsReturned != null)
            {
                sql += "AND A.[Returned] = @IsReturned ";
                parameters.Add("IsReturned", request.IsReturned);
            }

            if (request.OrderBy != null)
            {
                sql += "ORDER BY " + GetOrderBy(request.OrderBy);
            }
            else
            {
                sql += "ORDER BY A.[RentalDate]";
            }

            sql += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            parameters.Add("Offset", (request.PageNumber - 1) * request.PageSize);
            parameters.Add("PageSize", request.PageSize);

            var result = (await connection.QueryAsync<PreviousRentalOwnerDto>(sql, parameters)).AsList();

            var groupedResults = result.GroupBy(r => new { r.LibraryName, r.RentalId, r.RentalDate, r.Returned, r.UserName })
            .Select(group => new ResultPreviousRentals
            {
                LibraryName = group.Key.LibraryName,
                BookNames = group.Select(r => r.BookName).ToList(),
                RentalId = group.Key.RentalId,
                RentalDate = group.Key.RentalDate,
                Returned = group.Key.Returned,
                UserName = group.Key.UserName
            })
            .ToList();


            var totalRecords = groupedResults.Count();
            var paginatedResult = new PaginationResult<ResultPreviousRentals>(request.PageNumber, request.PageSize, totalRecords, groupedResults);

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
                    case "RentalDate":
                        sqlOrderByClauses.Add("A.[RentalDate]");
                        break;
                    case "-RentalDate":
                        sqlOrderByClauses.Add("A.[RentalDate] DESC");
                        break;
                    case "UserName":
                        sqlOrderByClauses.Add("U.[Username]");
                        break;
                    case "-UserName":
                        sqlOrderByClauses.Add("U.[Username] DESC");
                        break;
                    case "LibraryName":
                        sqlOrderByClauses.Add("L.[LibraryName]");
                        break;
                    case "-LibraryName":
                        sqlOrderByClauses.Add("L.[LibraryName] DESC");
                        break;

                    default:
                        sqlOrderByClauses.Add("A.[RentalDate]");
                        break;
                }
            }
            if (sqlOrderByClauses.Count == 0)
            {
                sqlOrderByClauses.Add("A.[RentalDate]");
            }
            return string.Join(", ", sqlOrderByClauses);
        }

    }
}
