namespace Catalog.Domain.RentalBooks
{
    public class CommentedBookDto
    {
        public string LibraryName { get; set; }
        public string BookTitle { get; set; }
        public string Comment { get; set; }
        public bool IsCommentApproved { get; set; }
        public bool IsCommentReported { get; set; }
        public string ReaderUsername { get; set; }
    }
}
