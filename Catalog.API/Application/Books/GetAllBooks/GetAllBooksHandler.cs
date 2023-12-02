using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Infrastructure.Configuration.Commands;

namespace Catalog.API.Application.Books.GetAllBooks
{
    public class GetAllBooksHandler : ICommandHandler<GetAllBooks, PaginationResult<Book>>
    {
        private readonly IBookRepository _bookRepository;

        public GetAllBooksHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<PaginationResult<Book>> Handle(GetAllBooks request, CancellationToken cancellationToken)
        {
            return await _bookRepository.Get(request.PaginationFilter);
        }
    }
}
