using BuildingBlocks.Application.Data;
using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Domain.Owners;
using Catalog.Infrastructure.Configuration.Queries;
using Dapper;

namespace Catalog.API.Application.Rentals.TopTenMostPopularBooksOwner
{
    public class TopTenMostPopularBooksOwnerQueryHandler : IQueryHandler<TopTenMostPopularBooksOwnerQuery, Result>
    {

        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly IOwnerRepository _ownerRepository;
        public TopTenMostPopularBooksOwnerQueryHandler(ISqlConnectionFactory connectionFactory, IOwnerRepository ownerRepository)
        {
            _connectionFactory = connectionFactory;
            _ownerRepository = ownerRepository;
        }

        public async Task<Result> Handle(TopTenMostPopularBooksOwnerQuery request, CancellationToken cancellationToken)
        {
            var connection = _connectionFactory.GetOpenConnection();
            var owner = await _ownerRepository.GetByUsername(request.OwnerUsername);
            if (owner == null)
            {
                return Result.Failure("This owner does not exist.");
            }
            var parameters = new { OwnerId = owner.OwnerId, StartDate = request.StartDate, EndDate = request.EndDate };


            string sql = @"
            SELECT TOP 10
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
            WHERE B.[BookId] IN (
                SELECT TOP 10 RB.[BookId]
                FROM [Libraries] AS [L]
                JOIN [Books] AS [BB] ON L.[LibraryId] = BB.[LibraryId]
                JOIN [RentalBooks] AS [RB] ON BB.[BookId] = RB.[BookId]
                JOIN [Rentals] AS [R] ON RB.[RentalId] = R.[RentalId]
                WHERE R.[RentalDate] BETWEEN @StartDate AND @EndDate AND L.[OwnerId] = @OwnerId
                GROUP BY RB.[BookId]
                ORDER BY COUNT(*) DESC
            );";


            var result = (await connection.QueryAsync<BookDto>(sql, parameters)).AsList();
            return Result.Success(result);
        }
    }
}
