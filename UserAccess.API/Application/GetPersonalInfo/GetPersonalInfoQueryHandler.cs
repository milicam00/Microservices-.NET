using BuildingBlocks.Application.Data;
using BuildingBlocks.Domain;
using Dapper;
using UserAccess.Domain.Users;
using UserAccess.Infrastructure.Configuration.Queries;

namespace UserAccess.API.Application.GetPersonalInfo
{
    public class GetPersonalInfoQueryHandler : IQueryHandler<GetPersonalInfoQuery, Result>
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly IUserRepository _userRepository;
        public GetPersonalInfoQueryHandler(ISqlConnectionFactory connectionFactory, IUserRepository userRepository)
        {
            _connectionFactory = connectionFactory;
            _userRepository = userRepository;
        }
        public async Task<Result> Handle(GetPersonalInfoQuery request, CancellationToken cancellationToken)
        {
            User user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null)
            {
                return Result.Failure("This user does not exist.");
            }

            var connection = _connectionFactory.GetOpenConnection();
            var parameter = new { Username = request.Username };
            string localHost = "https://localhost:44319";

            string sql = "SELECT " +
            $"U.[UserName] AS [{nameof(PersonalInfoDto.UserName)}], " +
            $"U.[Email] AS [{nameof(PersonalInfoDto.Email)}], " +
            $"U.[FirstName] AS [{nameof(PersonalInfoDto.FirstName)}], " +
            $"U.[LastName] AS [{nameof(PersonalInfoDto.LastName)}], " +
            $"U.[RegisterDate] AS [{nameof(PersonalInfoDto.RegisterDate)}], " +
           $"CONCAT('{localHost}', PI.[Path]) AS [{nameof(PersonalInfoDto.ProfileImageUrl)}] " +
            "FROM [Users] AS [U] " +
            "LEFT JOIN [ProfileImages] AS [PI] ON U.[ProfileImageId] = PI.[ProfileImageId] " +
            "WHERE [UserName] = @Username ";

            var result = (await connection.QueryAsync<PersonalInfoDto>(sql, parameter)).FirstOrDefault();
            if (result == null)
            {
                return Result.Failure("Error");
            }
            return Result.Success(result);
        }
    }
}
