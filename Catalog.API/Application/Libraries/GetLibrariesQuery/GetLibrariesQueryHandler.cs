using BuildingBlocks.Application.Data;
using Catalog.Domain.Libraries;
using Catalog.Infrastructure.Configuration.Queries;
using Dapper;

namespace Catalog.API.Application.Libraries.GetLibrariesQuery
{
    public class GetLibrariesQueryHandler : IQueryHandler<GetLibrariesQuery, List<LibraryDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetLibrariesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<List<LibraryDto>> Handle(GetLibrariesQuery query, CancellationToken cancellationToken)
        {
            var connection = _sqlConnectionFactory.GetOpenConnection();

            string sql = "SELECT " +
                $"[LibraryId] AS [{nameof(LibraryDto.LibraryId)}], " +
                $"[LibraryName] AS [{nameof(LibraryDto.Name)}], " +
                $"[IsActive] AS [{nameof(LibraryDto.IsActive)}] " +
                 "FROM [Libraries] AS [Libraries] ";

            return (await connection.QueryAsync<LibraryDto>(sql)).AsList();
        }
    }
}
