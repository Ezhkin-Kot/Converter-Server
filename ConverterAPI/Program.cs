using ConverterAPI;
using System.Threading.Tasks.Sources;
using Microsoft.OpenApi.Models;
using ConverterAPI.DB;
using Microsoft.AspNetCore.Mvc;
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

//app.UseHttpsRedirection();

app.MapGet("/users", () => User.GetUsers());
app.MapGet("/users/{id:int}", (int id) => User.GetUser(id));
app.MapPost("/users", (string newUser) => User.CreateUser(newUser));
app.MapPut("/users", (string updUser) => User.UpdateUser(updUser));
app.MapDelete("/users/{id:int}", (int id) => User.RemoveUser(id));

app.Run();