using BuildingBlocks.Application.Data;
using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Infrastructure.Configuration.Queries;
using Dapper;

namespace Catalog.API.Application.Books.GetBook
{
    public class GetBookQueryHandler : IQueryHandler<GetBookQuery, Result>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public GetBookQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }
        public async Task<Result> Handle(GetBookQuery query, CancellationToken cancellationToken)
        {
            int pageNumber = query.PageNumber;
            int pageSize = query.PageSize;
            var connection = _sqlConnectionFactory.GetOpenConnection();
            string localHost = "https://localhost:44319";
            string sql = "SELECT " +
            $"B.[BookId] AS [{nameof(BookDto.BookId)}], " +
            $"B.[Title] AS [{nameof(BookDto.Title)}], " +
            $"B.[Description] AS [{nameof(BookDto.Description)}], " +
            $"B.[Author] AS [{nameof(BookDto.Author)}], " +
            $"B.[Pages] AS [{nameof(BookDto.Pages)}], " +
            $"B.[Genres] AS [{nameof(BookDto.Genres)}], " +
            $"B.[PubblicationYear] AS [{nameof(BookDto.PubblicationYear)}], " +
            $"B.[UserRating] AS [{nameof(BookDto.UserRating)}], " +
             $"CONCAT('{localHost}', I.[Path]) AS [{nameof(BookDto.ImageUrl)}] " +
            "FROM [Books] AS [B] " +
            "LEFT JOIN [Images] AS [I] ON B.[ImageId] = I.[ImageId] " +
            "WHERE 1 = 1 ";

            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(query.Title))
            {
                var lowerQueryTitle = query.Title.ToLower();
                sql += "AND LOWER(B.[Title]) LIKE @Title + '%' ";
                parameters.Add("Title", "%" + lowerQueryTitle + "%");
            }

            if (!string.IsNullOrEmpty(query.Author))
            {
                var lowerQueryAuthor = query.Author.ToLower();
                sql += "AND LOWER(B.[Author]) LIKE @Author + '%' ";
                parameters.Add("Author", "%" + lowerQueryAuthor + "%");
            }

            if (query.PubblicationYear.HasValue)
            {
                sql += "AND B.[PubblicationYear] = @PubblicationYear ";
                parameters.Add("PubblicationYear", query.PubblicationYear.Value);
            }

            if (query.Genres.HasValue)
            {
                sql += "AND B.[Genres] = @Genres ";
                parameters.Add("Genres", query.Genres.Value);
            }

            if (query.Pages.HasValue)
            {
                sql += "AND B.[Pages] = @Pages ";
                parameters.Add("Pages", query.Pages.Value);
            }

            if (query.Rate.HasValue)
            {
                sql += "AND B.[UserRating] >= @Rate AND B.[UserRating] < (@Rate + 1) ";
                parameters.Add("Rate", query.Rate.Value);
            }

            sql += "AND B.[LibraryId] IN (SELECT [LibraryId] FROM [Libraries] WHERE [IsActive] = 1) " +
                "AND B.[IsDeleted] = 0 ";

            if (query.OrderBy != null)
            {
                sql += "ORDER BY " + GetOrderBy(query.OrderBy);
            }
            else
            {
                sql += "ORDER BY B.[BookId]";
            }
            sql += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

            parameters.Add("Offset", (pageNumber - 1) * pageSize);
            parameters.Add("PageSize", pageSize);
            parameters.Add("OrderBy", query.OrderBy);

            return Result.Success((await connection.QueryAsync<BookDto>(sql, parameters)).AsList());
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
                    case "Author":
                        sqlOrderByClauses.Add("B.[Author]");
                        break;
                    case "-Author":
                        sqlOrderByClauses.Add("B.[Author] DESC");
                        break;
                    case "PubblicationYear":
                        sqlOrderByClauses.Add("B.[PubblicationYear]");
                        break;
                    case "-PubblicationYear":
                        sqlOrderByClauses.Add("B.[PubblicationYear] DESC");
                        break;
                    case "Rate":
                        sqlOrderByClauses.Add("B.[UserRating]");
                        break;
                    case "-Rate":
                        sqlOrderByClauses.Add("B.[UserRating] DESC");
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
