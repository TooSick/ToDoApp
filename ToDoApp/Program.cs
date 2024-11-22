using FluentValidation;
using FluentValidation.AspNetCore;
using ToDoApp.API.Validators;
using ToDoApp.Application.Services;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Filters;
using ToDoApp.Domain.Interfaces;
using ToDoApp.Infrastructure.Database;
using ToDoApp.Infrastructure.Providers;
using ToDoApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateToDoItemViewModelValidator>();

// Add swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDatabaseInitializer, DatabaseInitializer>();
builder.Services.AddSingleton<ISqlProvider, SqlServerProvider>();
builder.Services.AddScoped<IDatabaseConnection, SqlServerConnection>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("ToDoAppConnection");
    return new SqlServerConnection(connectionString);
});
builder.Services.AddScoped<IRepository<ToDoItem, ToDoFilter>, ToDoRepository>();
builder.Services.AddScoped<IService<ToDoItem, ToDoFilter>, ToDoService>();

var app = builder.Build();

var databaseInitializer = app.Services.GetRequiredService<IDatabaseInitializer>();
var masterConnectionString = builder.Configuration.GetConnectionString("MasterConnection");
var toDoAppConnectionString = builder.Configuration.GetConnectionString("ToDoAppConnection");
await databaseInitializer.CreateDatabaseAsync(masterConnectionString);
await databaseInitializer.CreateTableAsync(toDoAppConnectionString);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.MapControllers();

app.Run();