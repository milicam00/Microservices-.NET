using BuildingBlocks.Application.Data;
using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Domain.Libraries;
using Catalog.Domain.Owners;
using Catalog.Infrastructure.Configuration.Queries;
using Dapper;

namespace Catalog.API.Application.Rentals.TopFiveRatedBooksOfLibraryOwner
{
    public class TopFiveRatedBooksOfLibraryOwnerQueryHandler : IQueryHandler<TopFiveRatedBooksOfLibraryOwnerQuery, Result>
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly IOwnerRepository _ownerRepository;
        private readonly ILibraryRepository _libraryRepository;
        public TopFiveRatedBooksOfLibraryOwnerQueryHandler(ISqlConnectionFactory connectionFactory, IOwnerRepository ownerRepository, ILibraryRepository libraryRepository)
        {
            _connectionFactory = connectionFactory;
            _ownerRepository = ownerRepository;
            _libraryRepository = libraryRepository;
        }

        public async Task<Result> Handle(TopFiveRatedBooksOfLibraryOwnerQuery request, CancellationToken cancellationToken)
        {
            var connection = _connectionFactory.GetOpenConnection();
            var owner = await _ownerRepository.GetByUsername(request.OwnerUsername);
            if (owner == null)
            {
                return Result.Failure("This owner does not exist.");
            }
            var parameters = new { OwnerId = owner.OwnerId, LibraryId = request.LibraryId, StartDate = request.StartDate, EndDate = request.EndDate };



            var library = await _libraryRepository.GetByIdAsync(request.LibraryId);
            if (library == null)
            {
                return Result.Failure("This library doesn not exist.");
            }

            string sql = @"
            SELECT TOP 5
                B.[BookId] AS [BookId], 
                B.[Title] AS [Title], 
                B.[Description] AS [Description], 
                B.[Author] AS [Author], 
                B.[Pages] AS [Pages], 
                B.[Genres] AS [Genres], 
                B.[PubblicationYear] AS [PubblicationYear], 
                B.[UserRating] AS [UserRating], 
                B.[NumberOfCopies] AS [NumberOfCopies]
                FROM [Books] AS [B]
                WHERE B.[UserRating] IS NOT NULL
                AND B.[BookId] IN (
                SELECT TOP 5 B.[BookId]
                FROM [Libraries] AS [L]
                LEFT JOIN [Books] AS [B] ON L.[LibraryId] = B.[LibraryId]
                LEFT JOIN [RentalBooks] AS [RB] ON B.[BookId] = RB.[BookId]
                LEFT JOIN [Rentals] AS [R] ON RB.[RentalId] = R.[RentalId]
                WHERE B.[UserRating] IS NOT NULL AND [R].[RentalDate] BETWEEN @StartDate AND @EndDate AND L.[OwnerId]=@OwnerId AND L.[LibraryId]=@LibraryId
                GROUP BY B.[BookId]
                ORDER BY AVG(B.[UserRating]) DESC
            );";

            var result = (await connection.QueryAsync<BookDto>(sql, parameters)).AsList();
            return Result.Success(result);
        }
    }
}
