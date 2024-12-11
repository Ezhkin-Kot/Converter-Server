using ConverterAPI.DB.Session;
using ConverterAPI.DB.User;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.MapGet("/users", () => UserDb.GetUsers());
app.MapGet("/users/{id:int}", (int id) => UserDb.GetUserById(id));
app.MapPost("/users", ([FromBody] NewUser newUser) => UserDb.CreateUser(newUser));
app.MapPost("/users/auth", ([FromBody] NewUser newUser) => SessionDb.AuthUser(newUser));
app.MapPut("/users", ([FromBody] User updUser) => UserDb.UpdateUser(updUser));
app.MapDelete("/users/{id:int}", (int id) => UserDb.DeleteUser(id));

app.Run();