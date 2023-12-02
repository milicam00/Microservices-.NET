namespace UserAccess.API.Application.GetPersonalInfo
{
    public class PersonalInfoDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegisterDate { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
