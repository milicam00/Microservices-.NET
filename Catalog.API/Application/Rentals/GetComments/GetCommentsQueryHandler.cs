using BuildingBlocks.Application.Data;
using BuildingBlocks.Domain;
using Catalog.Domain.Owners;
using Catalog.Domain.RentalBooks;
using Catalog.Infrastructure.Configuration.Queries;
using Dapper;

namespace Catalog.API.Application.Rentals.GetComments
{
    public class GetCommentsQueryHandler : IQueryHandler<GetCommentsQuery, Result>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        private readonly IOwnerRepository _ownerRepository;
        public GetCommentsQueryHandler(ISqlConnectionFactory sqlConnectionFactory, IOwnerRepository ownerRepository)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
            _ownerRepository = ownerRepository;
        }

        public async Task<Result> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
        {
            var owner = await _ownerRepository.GetByUsername(request.OwnerUsername);
            if (owner == null)
            {
                return Result.Failure("This owner does not exist");
            }

            var connection = _sqlConnectionFactory.GetOpenConnection();
            var parameters = new DynamicParameters();
            parameters.Add("OwnerId", owner.OwnerId);


            string sql = "SELECT " +
            $"L.[LibraryName] AS [{nameof(CommentedBookDto.LibraryName)}], " +
            $"B.[Title] AS [{nameof(CommentedBookDto.BookTitle)}], " +

            $"R.[TextualComment] AS [{nameof(CommentedBookDto.Comment)}], " +
            $"R.[IsCommentApproved] AS [{nameof(CommentedBookDto.IsCommentApproved)}], " +
            $"R.[IsCommentReported] AS [{nameof(CommentedBookDto.IsCommentReported)}], " +
            $"U.[UserName] AS [{nameof(CommentedBookDto.ReaderUsername)}] " +
            "FROM [Libraries] AS [L] " +
            "LEFT JOIN [Books] AS [B] ON L.[LibraryId] = B.[LibraryId] " +
            "LEFT JOIN [RentalBooks] AS [R] ON B.[BookId] = R.[BookId] " +
            "LEFT JOIN [Rentals] AS [A] ON R.[RentalId] = A.[RentalId] " +
            "LEFT JOIN [Readers] AS [U] ON A.[ReaderId] = U.[ReaderId] " +
            "WHERE [L].[OwnerId] = @OwnerId AND R.[TextualComment] IS NOT NULL ";



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
                sql += "ORDER BY [BookId]";
            }

            sql += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            parameters.Add("Offset", (request.PageNumber - 1) * request.PageSize);
            parameters.Add("PageSize", request.PageSize);
            parameters.Add("OrderBy", request.OrderBy);

            var result = (await connection.QueryAsync<CommentedBookDto>(sql, parameters)).AsList();

            var totalRecords = result.Count();
            var paginatedResult = new PaginationResult<CommentedBookDto>(request.PageNumber, request.PageSize, totalRecords, result);

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
                    case "LibraryName":
                        sqlOrderByClauses.Add("L.[LibraryName]");
                        break;
                    case "-LibraryName":
                        sqlOrderByClauses.Add("L.[LibraryName] DESC");
                        break;
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
                    default:
                        sqlOrderByClauses.Add("B.[BookId]");
                        break;
                }
            }
            if (sqlOrderByClauses.Count == 0)
            {
                sqlOrderByClauses.Add("B.[BookId]");
            }
            return string.Join(", ", sqlOrderByClauses);
        }
    }
}
