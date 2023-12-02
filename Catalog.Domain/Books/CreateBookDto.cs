namespace Catalog.Domain.Books
{
    public class CreateBookDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public int Pages { get; set; }
        public Genres Genres { get; set; }
        public int PubblicationYear { get; set; }
        public int NumberOfCopies { get; set; }
        public Guid LibraryId { get; set; }
    }
}
