using BuildingBlocks.Domain;
using UserAccess.Infrastructure.Contracts;

namespace UserAccess.API.Application.GetPersonalInfo
{
    public class GetPersonalInfoQuery : QueryBase<Result>
    {
        public GetPersonalInfoQuery(string username)
        {
            Username = username;
        }
        public string Username { get; set; }
    }
}
