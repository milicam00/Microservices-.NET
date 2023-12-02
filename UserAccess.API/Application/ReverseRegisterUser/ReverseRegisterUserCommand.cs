using BuildingBlocks.Domain;
using UserAccess.Infrastructure.Contracts;

namespace UserAccess.API.Application.ReverseRegisterUser
{
    public class ReverseRegisterUserCommand : CommandBase<Result>
    {
        public string Username { get; set; }
        public ReverseRegisterUserCommand(string username)
        {
            Username = username;
        }
    }
}
