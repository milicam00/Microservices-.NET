using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Domain.Rentals;

namespace Catalog.Domain.RentalBooks
{
    public class RentalBook : Entity
    {
        public Guid RentalBookId { get; set; }
        public Guid RentalId { get; set; }
        public Guid BookId { get; set; }
        public int? RatedRating { get; set; }
        public string? TextualComment { get; set; }
        public Rental Rental { get; set; }
        public Book Book { get; set; }
        public bool? IsCommentReported { get; set; }
        public bool? IsCommentApproved { get; set; }

        public RentalBook()
        {
            RentalId = Guid.NewGuid();

        }
        public RentalBook(Guid bookId)
        {
            BookId = bookId;

        }
        public void RateBook(int rate, string text)
        {

            IsCommentReported = false;
            IsCommentApproved = true;
            RatedRating = rate;
            TextualComment = text;

        }

        public void ReportComment()
        {
            IsCommentReported = true;
        }

        public void ApproveComment()
        {
            IsCommentApproved = true;
        }

        public void BlockComment()
        {
            IsCommentApproved = false;
        }

    }
}
