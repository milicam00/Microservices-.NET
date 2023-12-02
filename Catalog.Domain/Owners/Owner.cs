using BuildingBlocks.Domain;
using Catalog.Domain.Libraries;

namespace Catalog.Domain.Owners
{
    public class Owner : Entity
    {
        public Guid OwnerId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsBlocked { get; set; }

        public List<Library> Libraries { get; set; }

        public Owner()
        {
            OwnerId = Guid.NewGuid();
            IsBlocked = false;
        }

        public Owner(Guid ownerId, string userName, string email, string firstName, string lastName)
        {
            OwnerId = ownerId;
            UserName = userName;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            IsBlocked = false;
        }

        public static Owner CreateOwner(string userName, string email, string firstName, string lastName)
        {
            return new Owner(
                Guid.NewGuid(),
                userName,
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
            UserName = username;
        }

        public void ChangeFirstName(string firstName)
        {
            FirstName = firstName;
        }

        public void ChangeLastName(string lastName)
        {
            LastName = lastName;
        }
    }
}
