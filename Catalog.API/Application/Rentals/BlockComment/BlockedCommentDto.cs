namespace Catalog.API.Application.Rentals.BlockComment
{
    public class BlockedCommentDto
    {
        public string Comment { get; set; }
        public bool? IsCommentApproved { get; set; }
    }
}
