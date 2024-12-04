using Microsoft.OpenApi.Models;
using ConverterAPI.DB;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/users", () => User.GetUsers());
app.MapPost("/users", (UserDb user) => User.CreateUser(user));
app.MapPut("/users", (UserDb user) => User.UpdateUser(user));
app.MapDelete("/users/{id}", (int id) => User.RemoveUser(id));

app.Run();

app.Run();