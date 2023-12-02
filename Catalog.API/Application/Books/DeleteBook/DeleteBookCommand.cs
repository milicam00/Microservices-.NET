using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Books.DeleteBook
{
    public class DeleteBookCommand : CommandBase<Result>
    {
        public DeleteBookCommand(string ownerUsername, Guid bookId)
        {
            BookId = bookId;
            OwnerUsername = ownerUsername;
        }
        public Guid BookId { get; set; }
        public string OwnerUsername { get; set; }
    }
}
