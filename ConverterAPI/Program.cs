using ConverterAPI;
using System.Threading.Tasks.Sources;
using Microsoft.OpenApi.Models;
using ConverterAPI.DB;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Get the connection string
string connectionString = ConfigurationHelper.GetConnectionString("DefaultConnection");

// Connect to the PostgreSQL server
await using var conn = new NpgsqlConnection(connectionString);
await conn.OpenAsync();

Console.WriteLine($"The PostgreSQL version: {conn.PostgreSqlVersion}");

//app.UseHttpsRedirection();

app.MapGet("/users", () => User.GetUsers());
app.MapGet("/users/{id}", (int id) => User.GetUser(id));
app.MapPost("/users", (UserDb user) => User.CreateUser(user));
app.MapPut("/users", (UserDb user) => User.UpdateUser(user));
app.MapDelete("/users/{id}", (int id) => User.RemoveUser(id));

app.Run();