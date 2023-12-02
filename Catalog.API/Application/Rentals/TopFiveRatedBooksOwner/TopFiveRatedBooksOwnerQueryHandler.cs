using BuildingBlocks.Application.Data;
using BuildingBlocks.Domain;
using Catalog.Domain.Books;
using Catalog.Domain.Owners;
using Catalog.Infrastructure.Configuration.Queries;
using Dapper;

namespace Catalog.API.Application.Rentals.TopFiveRatedBooksOwner
{
    public class TopFiveRatedBooksOwnerQueryHandler : IQueryHandler<TopFiveRatedBooksOwnerQuery, Result>
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly IOwnerRepository _ownerRepository;
        public TopFiveRatedBooksOwnerQueryHandler(ISqlConnectionFactory connectionFactory, IOwnerRepository ownerRepository)
        {
            _connectionFactory = connectionFactory;
            _ownerRepository = ownerRepository;
        }

        public async Task<Result> Handle(TopFiveRatedBooksOwnerQuery request, CancellationToken cancellationToken)
        {
            var connection = _connectionFactory.GetOpenConnection();
            Owner owner = await _ownerRepository.GetByUsername(request.OwnerUsername);
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
                WHERE B.[UserRating] IS NOT NULL
                AND B.[BookId] IN (
                SELECT TOP 10 B.[BookId]
                FROM [Libraries] AS [L]
                LEFT JOIN [Books] AS [B] ON L.[LibraryId] = B.[LibraryId]
                LEFT JOIN [RentalBooks] AS [RB] ON B.[BookId] = RB.[BookId]
                LEFT JOIN [Rentals] AS [R] ON RB.[RentalId] = R.[RentalId]
                WHERE B.[UserRating] IS NOT NULL AND [R].[RentalDate] BETWEEN @StartDate AND @EndDate AND L.[OwnerId]=@OwnerId
                GROUP BY B.[BookId]
                ORDER BY AVG(B.[UserRating]) DESC
            );";


            var result = (await connection.QueryAsync<BookDto>(sql, parameters)).AsList();
            return Result.Success(result);
        }
    }
}
