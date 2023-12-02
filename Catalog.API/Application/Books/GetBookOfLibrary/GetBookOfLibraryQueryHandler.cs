using BuildingBlocks.Application.Data;
using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Infrastructure.Configuration.Queries;
using Dapper;

namespace Catalog.API.Application.Books.GetBookOfLibrary
{
    public class GetBooksOfLibraryQueryHandler : IQueryHandler<GetBooksOfLibraryQuery, Result>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetBooksOfLibraryQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result> Handle(GetBooksOfLibraryQuery query, CancellationToken cancellationToken)
        {
            int pageNumber = query.PageNumber;
            int pageSize = query.PageSize;
            var connection = _sqlConnectionFactory.GetOpenConnection();


            string sql = "SELECT " +
               $"[BookId] AS [{nameof(BookDto.BookId)}], " +
               $"[Title] AS [{nameof(BookDto.Title)}], " +
               $"[Description] AS [{nameof(BookDto.Description)}], " +
               $"[Author] AS [{nameof(BookDto.Author)}], " +
               $"[Pages] AS [{nameof(BookDto.Pages)}], " +
               $"[Genres] AS [{nameof(BookDto.Genres)}], " +
               $"[PubblicationYear] AS [{nameof(BookDto.PubblicationYear)}], " +
               $"[UserRating] AS [{nameof(BookDto.UserRating)}] " +
               "FROM [Books] AS [Books] " +
               "WHERE [LibraryId] = @LibraryId ";

            var parameters = new DynamicParameters();
            parameters.Add("LibraryId", query.LibraryId);

            if (!string.IsNullOrEmpty(query.Title))
            {
                var lowerQueryTitle = query.Title.ToLower();
                sql += "AND LOWER([Title]) LIKE @Title + '%' ";
                parameters.Add("Title", "%" + lowerQueryTitle + "%");
            }

            if (!string.IsNullOrEmpty(query.Author))
            {
                var lowerQueryAuthor = query.Author.ToLower();
                sql += "AND LOWER([Author]) LIKE @Author + '%' ";
                parameters.Add("Author", "%" + lowerQueryAuthor + "%");
            }

            if (query.PubblicationYear.HasValue)
            {
                sql += "AND [PubblicationYear] = @PubblicationYear ";
                parameters.Add("PubblicationYear", query.PubblicationYear.Value);
            }

            if (query.Genres.HasValue)
            {
                sql += "AND [Genres] = @Genres ";
                parameters.Add("Genres", query.Genres.Value);
            }

            if (query.Pages.HasValue)
            {
                sql += "AND [Pages] = @Pages ";
                parameters.Add("Pages", query.Pages.Value);
            }

            if (query.Rate.HasValue)
            {
                sql += "AND [UserRating] >= @Rate AND [UserRating] < (@Rate + 1) ";
                parameters.Add("Rate", query.Rate.Value);
            }
            sql += "AND [IsDeleted] = 0 ";

            if (query.OrderBy != null)
            {
                sql += "ORDER BY " + GetOrderBy(query.OrderBy);
            }
            else
            {
                sql += "ORDER BY [BookId]";
            }
            sql += " OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

            parameters.Add("Offset", (pageNumber - 1) * pageSize);
            parameters.Add("PageSize", pageSize);
            parameters.Add("OrderBy", query.OrderBy);

            var items = (await connection.QueryAsync<BookDto>(sql, parameters)).AsList();
            var totalRecords = items.Count();
            var result = new PaginationResult<BookDto>(pageNumber, pageSize, totalRecords, items);

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
                    case "Title":
                        sqlOrderByClauses.Add("[Title]");
                        break;
                    case "-Title":
                        sqlOrderByClauses.Add("[Title] DESC");
                        break;
                    case "Author":
                        sqlOrderByClauses.Add("[Author]");
                        break;
                    case "-Author":
                        sqlOrderByClauses.Add("[Author] DESC");
                        break;
                    case "PubblicationYear":
                        sqlOrderByClauses.Add("[PubblicationYear]");
                        break;
                    case "-PubblicationYear":
                        sqlOrderByClauses.Add("[PubblicationYear] DESC");
                        break;
                    default:
                        sqlOrderByClauses.Add("[BookId]");
                        break;
                }
            }
            if (sqlOrderByClauses.Count == 0)
            {
                sqlOrderByClauses.Add("[BookId]");
            }
            return string.Join(", ", sqlOrderByClauses);
        }
    }
}
