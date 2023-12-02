using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Domain.Books
{
    public class BookRepository : IBookRepository
    {
        private readonly CatalogContext _catalogContext;

        public BookRepository(CatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        public async Task AddAsync(Book book)
        {
            await _catalogContext.Books.AddAsync(book);
        }

        public async Task AddBooksAsync(List<Book> books)
        {
            foreach (var book in books)
            {
                await _catalogContext.Books.AddAsync(book);
            }
        }

        public async Task<PaginationResult<Book>> Get(PaginationFilter filter)
        {
            if (filter.PageNumber <= 0 || filter.PageSize <= 0)
            {
                throw new ArgumentException("Invalid page number or page size.");
            }

            var totalRecords = await _catalogContext.Books.CountAsync();

            var pagedData = await _catalogContext.Books
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

            PaginationResult<Book> result = new PaginationResult<Book>(filter.PageNumber, filter.PageSize, totalRecords, pagedData);

            return result;
        }

        public async Task<Book?> GetByIdAsync(Guid bookId)
        {
            return await _catalogContext.Books.FirstOrDefaultAsync(x => x.BookId == bookId);
        }

        public async Task<List<Book>> GetByIdsAsync(List<Guid> bookIds)
        {
            return await _catalogContext.Books.Where(book => bookIds.Contains(book.BookId)).ToListAsync();
        }

        public void UpdateBook(Book book)
        {
            _catalogContext.Books.Update(book);
        }

        public void UpdateBooks(List<Book> books)
        {
            foreach (var book in books)
            {
                _catalogContext.Books.Update(book);
            }
        }
    }
}
