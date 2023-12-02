using BuildingBlocks.Domain;

namespace Catalog.Domain.Readers
{
    public class Reader : Entity
    {
        public Guid ReaderId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsBlocked { get; set; }

        public Reader()
        {
            ReaderId = Guid.NewGuid();
            IsBlocked = false;
        }
        public Reader(Guid readerId, string userName, string email, string firstName, string lastName)
        {
            ReaderId = readerId;
            UserName = userName;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            IsBlocked = false;

        }

        public static Reader CreateUser(string username, string email, string firstName, string lastName)
        {
            return new Reader(
                Guid.NewGuid(),
                username,
                email,
                firstName,
                lastName);

        }

        public void Block()
        {
            IsBlocked = true;
        }
        public void Unblock()
        {
            IsBlocked = false;
        }
        public void ChangeUsername(string username)
        {
            this.UserName = username;
        }
        public void ChangeFirstName(string firstName)
        {
            this.FirstName = firstName;
        }

        public void ChangeLastName(string lastName)
        {
            this.LastName = lastName;
        }
    }
}
