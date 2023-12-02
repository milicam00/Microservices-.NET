namespace API_Gateway.Controllers.Account.Requests
{
    public class ChangePersonalInfoRequest
    {
        public string? NewUsername { get; set; }
        public string? NewFirstName { get; set; }
        public string? NewLastName { get; set; }
    }
}
