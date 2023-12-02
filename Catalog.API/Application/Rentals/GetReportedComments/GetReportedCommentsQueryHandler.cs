using BuildingBlocks.Application.Data;
using BuildingBlocks.Domain;
using Catalog.Domain.Rentals;
using Catalog.Infrastructure.Configuration.Queries;
using Dapper;

namespace Catalog.API.Application.Rentals.GetReportedComments
{
    public class GetReportedCommentsQueryHandler : IQueryHandler<GetReportedCommentsQuery, Result>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public GetReportedCommentsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }
        public async Task<Result> Handle(GetReportedCommentsQuery request, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();
            var parameters = new DynamicParameters();


            string sql = "SELECT " +
                         $"R.[RentalBookId] AS [{nameof(ReportedCommentDto.RentalBookId)}], " +
                         $"B.[Title] AS [{nameof(ReportedCommentDto.BookTitle)}], " +
                         $"R.[RatedRating] AS [{nameof(ReportedCommentDto.Rate)}], " +
                         $"R.[TextualComment] AS [{nameof(ReportedCommentDto.Comment)}], " +
                         $"R.[IsCommentReported] AS [{nameof(ReportedCommentDto.IsCommentReported)}], " +
                         $"D.[UserName] AS [{nameof(ReportedCommentDto.Username)}] " +
                         "FROM [RentalBooks] AS [R] " +
                         "LEFT JOIN [Books] AS [B] ON R.[BookId] = B.[BookId] " +
                         "LEFT JOIN [Rentals] AS [A] ON R.[RentalId] = A.[RentalId] " +
                         "LEFT JOIN [Readers] AS [D] ON A.[ReaderId] = D.[ReaderId] " +
                         "WHERE [R].[IsCommentReported] = 1 ";

            if (request.OrderBy != null)
            {
                sql += "ORDER BY " + GetOrderBy(request.OrderBy);
            }
            else
            {
                sql += "ORDER BY B.[BookId]";
            }


            sql += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            parameters.Add("Offset", (request.PageNumber - 1) * request.PageSize);
            parameters.Add("PageSize", request.PageSize);
            parameters.Add("OrderBy", request.OrderBy);

            var result = (await connection.QueryAsync<ReportedCommentDto>(sql, parameters)).AsList();
            var totalRecords = result.Count();

            var paginatedResult = new PaginationResult<ReportedCommentDto>(request.PageNumber, request.PageSize, totalRecords, result);

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
                    case "Rate":
                        sqlOrderByClauses.Add("R.[RatedRating]");
                        break;
                    case "-Rate":
                        sqlOrderByClauses.Add("R.[RatedRating] DESC");
                        break;
                    case "UserName":
                        sqlOrderByClauses.Add("D.[UserName]");
                        break;
                    case "-UserName":
                        sqlOrderByClauses.Add("D.[UserName] DESC");
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
