using BuildingBlocks.Domain;
using UserAccess.Infrastructure.Contracts;

namespace UserAccess.API.Application.GetAllUsers
{
    public class GetAllUsersQuery : QueryBase<Result>
    {
        public GetAllUsersQuery(string? username, string? email, string? firstName, string? lastName, bool? isActive, int pageNumber, int pageSize)
        {
            UserName = username;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            IsActive = isActive;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? IsActive { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
