using Catalog.Domain.Libraries;
using Catalog.Infrastructure.Configuration.Queries;

namespace Catalog.API.Application.Libraries.GetLibrariesQuery
{
    public class GetLibrariesQuery : QueryBase<List<LibraryDto>>
    {
        public GetLibrariesQuery() { }
    }
}
