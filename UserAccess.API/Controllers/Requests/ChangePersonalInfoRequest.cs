namespace UserAccess.API.Controllers.Requests
{
    public class ChangePersonalInfoRequest
    {
        public string Username { get; set; }    
        public string? NewUsername { get; set; }
        public string? NewFirstName { get; set; }
        public string? NewLastName { get; set; }
    }
}
