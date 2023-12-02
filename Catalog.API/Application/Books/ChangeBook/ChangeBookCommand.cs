using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Books.ChangeBook
{
    public class ChangeBookCommand : CommandBase<Result>
    {
        public ChangeBookCommand(Guid bookId, string title, string description, string author, int pages, int pubblicationYear, double userRating, int numOfCopies, string username)
        {
            BookId = bookId;
            Title = title;
            Description = description;
            Author = author;
            Pages = pages;
            PubblicationYear = pubblicationYear;
            UserRating = userRating;
            NumberOfCopies = numOfCopies;
            Username = username;

        }
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public int Pages { get; set; }
        public int PubblicationYear { get; set; }
        public double UserRating { get; set; }
        public int NumberOfCopies { get; set; }
        public string Username { get; set; }

    }
}
