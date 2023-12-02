using BuildingBlocks.Application.Data;
using BuildingBlocks.Domain;
using Catalog.Domain.RentalBooks;
using Catalog.Infrastructure.Configuration.Queries;
using Dapper;

namespace Catalog.API.Application.Rentals.GetCommentsOfBook
{
    public class GetCommentsOfBookQueryHandler : IQueryHandler<GetCommentsOfBookQuery, Result>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public GetCommentsOfBookQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result> Handle(GetCommentsOfBookQuery request, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();
            var parameters = new DynamicParameters();



            string sql = @"
               SELECT 
               U.[UserName] AS [UserName],
               RB.[TextualComment] AS [TextualComment]          
               FROM [RentalBooks] AS [RB]
               JOIN [Rentals] AS [R] ON RB.[RentalId] = R.[RentalId]
               JOIN [Readers] AS [U] ON R.[ReaderId] = U.[ReaderId]
               WHERE RB.[BookId] = @BookId AND RB.[TextualComment] IS NOT NULL
            ";

            if (request.OrderBy != null)
            {
                sql += "ORDER BY " + GetOrderBy(request.OrderBy);
            }
            else
            {
                sql += "ORDER BY RB.[BookId]";
            }

            sql += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

            parameters.Add("BookId", request.BookId);
            parameters.Add("Offset", (request.PageNumber - 1) * request.PageSize);
            parameters.Add("PageSize", request.PageSize);
            parameters.Add("OrderBy", request.OrderBy);


            var result = (await connection.QueryAsync<CommentedBookDto>(sql, parameters)).AsList();
            return Result.Success(result);
        }

        private string GetOrderBy(string orderBy)
        {
            var orderByClauses = orderBy.Split(',');
            var sqlOrderByClauses = new List<string>();
            foreach (var clause in orderByClauses)
            {
                switch (clause.Trim())
                {
                    case "UserName":
                        sqlOrderByClauses.Add("U.[UserName]");
                        break;
                    case "-UserName":
                        sqlOrderByClauses.Add("U.[UserName] DESC");
                        break;

                    default:
                        sqlOrderByClauses.Add("RB.[BookId]");
                        break;
                }
            }
            if (sqlOrderByClauses.Count == 0)
            {
                sqlOrderByClauses.Add("RB.[BookId]");
            }

            return string.Join(", ", sqlOrderByClauses);
        }
    }
}
