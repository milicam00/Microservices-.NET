namespace API_Gateway.Controllers.Account.Requests
{
    public class NewPasswordRequest
    {
        public string Password { get; set; }
        public string Token { get; set; }
        public int Code { get; set; }
    }
}
