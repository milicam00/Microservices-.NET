using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Books.AddBooks
{
    public class AddBooksCommand : CommandBase<Result>
    {
        public AddBooksCommand(List<CreateBookDto> bookList, string ownerUsername)
        {
            BookList = bookList;
            OwnerUsername = ownerUsername;
        }
        public string OwnerUsername { get; set; }
        public List<CreateBookDto> BookList { get; set; }
    }
}
