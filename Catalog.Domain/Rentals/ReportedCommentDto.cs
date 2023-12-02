namespace Catalog.Domain.Rentals
{
    public class ReportedCommentDto
    {
        public Guid RentalBookId { get; set; }
        public string BookTitle { get; set; }
        public int Rate { get; set; }
        public string Comment { get; set; }
        public bool IsCommentReported { get; set; }
        public string Username { get; set; }
    }
}
