using BuildingBlocks.Application.Data;
using BuildingBlocks.Domain;
using Dapper;
using UserAccess.Infrastructure.Configuration.Queries;

namespace UserAccess.API.Application.GetAllAPIKeys
{
    public class GetAllAPIKeysQueryHandler : IQueryHandler<GetAllAPIKeysQuery, Result>
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        public GetAllAPIKeysQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Result> Handle(GetAllAPIKeysQuery request, CancellationToken cancellationToken)
        {
            var connection = _connectionFactory.GetOpenConnection();

            var parameters = new DynamicParameters();
            int pageNumber = request.PageNumber;
            int pageSize = request.PageSize;

            string sql = "SELECT " +
            $"[KeyId] AS [{nameof(APIKeyDto.APIKeyId)}], " +
            $"[Key] AS [{nameof(APIKeyDto.Key)}], " +
            $"[Name] AS [{nameof(APIKeyDto.Name)}], " +
            $"[UserName] AS [{nameof(APIKeyDto.Username)}] " +
            "FROM [APIKeys] AS [APIKeys] " +
            "WHERE 1 = 1 ";

            if (!string.IsNullOrEmpty(request.Name))
            {
                var lowerQueryTitle = request.Name.ToLower();
                sql += "AND LOWER([Name]) LIKE @Name + '%' ";
                parameters.Add("Name", "%" + lowerQueryTitle + "%");
            }

            sql += "ORDER BY [CreatedAt] OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";
            parameters.Add("Offset", (pageNumber - 1) * pageSize);
            parameters.Add("PageSize", pageSize);

            return Result.Success((await connection.QueryAsync<APIKeyDto>(sql, parameters)).AsList());

        }
    }
}
