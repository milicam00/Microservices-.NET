using BuildingBlocks.Domain;
using Catalog.Infrastructure.Contracts;

namespace Catalog.API.Application.Rentals.ReturnBooks
{
    public class ReturnBooksCommand : CommandBase<Result>
    {
        public string ReaderUsername { get; set; }
        public ReturnBooksCommand(string readerUsername)
        {
            ReaderUsername = readerUsername;
        }
    }
}
