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
app.MapGet("/sessions", () => SessionDb.GetSessions());
app.MapGet("/sessions/{userid:int}", (int userid) => SessionDb.GetSessionByUserId(userid));
app.MapPost("/users/reg", ([FromBody] NewUser newUser) => UserDb.CreateUser(newUser));
app.MapPost("/sessions/auth", ([FromBody] NewUser newUser) => SessionDb.AuthUser(newUser));
app.MapPut("/users", ([FromBody] User updUser) => UserDb.UpdateUser(updUser));
app.MapPut("/users/prem/{id:int}/{premium:bool}", (int id, bool premium) => UserDb.ChangePremium(id, premium));
app.MapPut("/sessions/close/{userid:int}", (int userid) => SessionDb.CloseSession(userid));
app.MapPut("/sessions/upd/{userid:int}", (int userid) => SessionDb.UpdAmount(userid));
app.MapDelete("/users/{id:int}", (int id) => UserDb.DeleteUser(id));
app.MapDelete("/sessions/{userid:int}", (int userid) => SessionDb.DeleteSession(userid));

app.Run();