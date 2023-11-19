using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VSASample.Data;
using VSASample.Entities;

namespace VSASample.Features.Books
{
    //public static class GetAllBooks
    //{
    //    public static void AddEndPoint(this IEndpointRouteBuilder app)
    //    {
    //        app.MapGet("/api/books", async (ISender sender) =>
    //        {
    //            var books = await sender.Send(new Query());

    //            return Results.Ok(books);
    //        });
    //    }
    //}

    public record Query : IRequest<List<Book>>;

    internal class QueryHandler : IRequestHandler<Query, List<Book>>
    {
        private readonly AppDbContext _dbContext;

        public QueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<Book>> Handle(Query request, CancellationToken cancellationToken)
        {
            var books = _dbContext.Books.AsNoTracking().ToList();

            return Task.FromResult(books);
        }
    }

    public class GetAllBooksEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/books", async (ISender sender) =>
            {
                var books = await sender.Send(new Query());

                return Results.Ok(books);
            });
        }
    }
}
