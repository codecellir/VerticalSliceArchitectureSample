using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VSASample.Data;
using VSASample.Features.Books;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddCarter();


var app = builder.Build();


//CreateBook.AddEndPoint(app);
//GetAllBooks.AddEndPoint(app);

app.MapCarter();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();


app.Run();

