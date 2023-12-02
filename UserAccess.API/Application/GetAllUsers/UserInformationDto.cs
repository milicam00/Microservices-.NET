namespace UserAccess.API.Application.GetAllUsers
{
    public class UserInformationDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool IsActive { get; set; }
        public string ImageUrl { get; set; }
    }
}
