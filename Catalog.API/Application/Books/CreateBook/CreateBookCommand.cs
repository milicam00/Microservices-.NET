using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Books.CreateBook
{
    public class CreateBookCommand : CommandBase<Result>
    {

        public CreateBookCommand(

            string title,
            string description,
            string author,
            int pages,
            Genres genres,
            int pubblicationYear,
            int numCopies,
            Guid libraryId
            )

        {

            Title = title;
            Description = description;
            Author = author;
            Pages = pages;
            Genres = genres;
            PubblicationYear = pubblicationYear;
            NumberOfCopies = numCopies;
            LibraryId = libraryId;

        }


        public Guid BookId { get; }

        public string Title { get; }

        public string Description { get; }

        public string Author { get; }

        public int Pages { get; }

        public Genres Genres { get; }

        public int PubblicationYear { get; }

        public int NumberOfCopies { get; }
        public Guid LibraryId { get; }

    }
}
