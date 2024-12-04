using Microsoft.OpenApi.Models;
using ConverterAPI.DB;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ConverterAPI", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ConverterAPI v1");
    });
}

app.MapGet("/users", () => UserDb.GetUsers());
app.MapPost("/users", (User user) => UserDb.CreateUser(user));
app.MapPut("/users", (User user) => UserDb.UpdateUser(user));
app.MapDelete("/users/{id}", (int id) => UserDb.RemoveUser(id));

app.Run();
