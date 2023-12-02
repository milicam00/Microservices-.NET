using BuildingBlocks.Domain;
using UserAccess.Infrastructure.Contracts;

namespace UserAccess.API.Application.ResetPasswordRequest
{
    public class ResetPasswordRequestCommand : CommandBase<Result>
    {
        public ResetPasswordRequestCommand(string username)
        {
            Username = username;
        }
        public string Username { get; set; }


    }
}
