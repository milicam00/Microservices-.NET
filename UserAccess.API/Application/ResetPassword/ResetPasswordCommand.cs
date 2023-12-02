using BuildingBlocks.Domain;
using UserAccess.Infrastructure.Contracts;

namespace UserAccess.API.Application.ResetPassword
{
    public class ResetPasswordCommand : CommandBase<Result>
    {
        public int Code { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public ResetPasswordCommand(int code, string token, string newPassword)
        {
            Code = code;
            Token = token;
            NewPassword = newPassword;
        }
    }
}
