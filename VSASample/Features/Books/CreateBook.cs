using Carter;
using FluentValidation;
using MediatR;
using VSASample.Data;
using VSASample.Entities;

namespace VSASample.Features.Books
{
    //public static class CreateBook
    //{
    //    public static void AddEndPoint(this IEndpointRouteBuilder app)
    //    {
    //        app.MapPost("/api/books", async (CreateBookCommand request, ISender sender) =>
    //        {
    //            var bookId = await sender.Send(request);

    //            return Results.Ok(bookId);
    //        });
    //    }
    //}

    public record CreateBookCommand(string Name, string Description) : IRequest<long>;

    public class Validator : AbstractValidator<CreateBookCommand>
    {
        public Validator()
        {

            RuleFor(d => d.Name)
                .NotEmpty();

            RuleFor(d => d.Description)
                .NotEmpty();

        }
    }

    internal class Handler : IRequestHandler<CreateBookCommand, long>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IValidator<CreateBookCommand> _validator;
        public Handler(AppDbContext appDbContext, IValidator<CreateBookCommand> validator)
        {
            _appDbContext = appDbContext;
            _validator = validator;
        }

        public Task<long> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                throw new BadHttpRequestException(string.Join(Environment.NewLine, validationResult.Errors.Select(d => d.ErrorMessage)));
            }
            var book = new Book
            {
                Name = request.Name,
                Description = request.Description,
            };

            _appDbContext.Books.Add(book);

            _appDbContext.SaveChanges();

            return Task.FromResult(book.Id);
        }
    }


    public class CreateBookEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/books", async (CreateBookCommand request, ISender sender) =>
            {
                var bookId = await sender.Send(request);

                return Results.Ok(bookId);
            });
        }
    }
}
