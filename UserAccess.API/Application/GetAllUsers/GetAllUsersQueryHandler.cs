using BuildingBlocks.Application.Data;
using BuildingBlocks.Domain;
using Dapper;
using UserAccess.Infrastructure.Configuration.Queries;

namespace UserAccess.API.Application.GetAllUsers
{
    public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, Result>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;
        public GetAllUsersQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }
        public async Task<Result> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();
            var parameters = new DynamicParameters();
            int pageNumber = request.PageNumber;
            int pageSize = request.PageSize;
            string localHost = "https://localhost:44319";

            string sql = "SELECT " +
           $"U.[UserId] AS [{nameof(UserInformationDto.UserId)}], " +
           $"U.[UserName] AS [{nameof(UserInformationDto.UserName)}], " +
           $"U.[Email] AS [{nameof(UserInformationDto.Email)}], " +
           $"U.[FirstName] AS [{nameof(UserInformationDto.FirstName)}], " +
           $"U.[LastName] AS [{nameof(UserInformationDto.LastName)}], " +
           $"U.[RegisterDate] AS [{nameof(UserInformationDto.RegisterDate)}], " +
           $"CONCAT('{localHost}', PI.[Path]) AS [{nameof(UserInformationDto.ImageUrl)}] " +
           "FROM [Users] AS [U] " +
           "LEFT JOIN [ProfileImages] AS [PI] ON U.[ProfileImageId] = PI.[ProfileImageId] " +
           "WHERE 1 = 1 ";

            if (!string.IsNullOrEmpty(request.UserName))
            {
                var lowerQueryTitle = request.UserName.ToLower();
                sql += "AND LOWER(U.[UserName]) LIKE @UserName + '%' ";
                parameters.Add("UserName", "%" + lowerQueryTitle + "%");
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                var lowerQueryTitle = request.Email.ToLower();
                sql += "AND LOWER(U.[Email]) LIKE @Email + '%' ";
                parameters.Add("Email", "%" + lowerQueryTitle + "%");
            }

            if (!string.IsNullOrEmpty(request.FirstName))
            {
                var lowerQueryTitle = request.FirstName.ToLower();
                sql += "AND LOWER(U.[FirstName]) LIKE @FirstName + '%' ";
                parameters.Add("FirstName", "%" + lowerQueryTitle + "%");
            }

            if (!string.IsNullOrEmpty(request.LastName))
            {
                var lowerQueryTitle = request.LastName.ToLower();
                sql += "AND LOWER(U.[LastName]) LIKE @LastName + '%' ";
                parameters.Add("LastName", "%" + lowerQueryTitle + "%");
            }

            if (request.IsActive.HasValue)
            {
                sql += "AND U.[IsActive] = @IsActive ";
                parameters.Add("IsActive", request.IsActive.Value);
            }

            sql += "ORDER BY U.[RegisterDate] OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

            parameters.Add("Offset", (pageNumber - 1) * pageSize);
            parameters.Add("PageSize", pageSize);

            return Result.Success((await connection.QueryAsync<UserInformationDto>(sql, parameters)).AsList());
        }
    }
}
