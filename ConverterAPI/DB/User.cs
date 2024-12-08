using System.Data;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace ConverterAPI.DB;
using ConverterAPI;
using System;
using Npgsql;

public record UserDb
{
    public int Id { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
    public bool Premium { get; set; }
}

public class User
{
    // Get the connection string
    private static string _connectionString = ConfigurationHelper.GetConnectionString("DefaultConnection");

    public static async Task<JsonResult> GetUsers()
    {
        var table = new DataTable();
        await using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            await using (var command = new NpgsqlCommand("SELECT * FROM users", connection))
            {
                var reader = await command.ExecuteReaderAsync();
                table.Load(reader);
                
                await reader.CloseAsync();
                await connection.CloseAsync();
            }
        }
        
        return new JsonResult(table);
    }

    public static async Task<JsonResult> GetUser(int id)
    {
        var table = new DataTable();
        await using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            await using (var command = new NpgsqlCommand("SELECT * FROM users WHERE id = @id", connection))
            {
                command.Parameters.AddWithValue("id", id);
                
                var reader = await command.ExecuteReaderAsync();
                table.Load(reader);
                
                await reader.CloseAsync();
                await connection.CloseAsync();
            }
        }
        
        return new JsonResult(table);
    }

    public static async Task<JsonResult> CreateUser(string jsonString)
    {
        var userDb = JsonSerializer.Deserialize<UserDb>(jsonString);
        var table = new DataTable();
        await using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            await using (var command = new NpgsqlCommand("INSERT INTO users (login, password) VALUES (@login, @password)", connection))
            {
                command.Parameters.AddWithValue("login", userDb.Login);
                command.Parameters.AddWithValue("password", userDb.Password);
                
                var reader = await command.ExecuteReaderAsync();
                table.Load(reader);
                
                await reader.CloseAsync();
                await connection.CloseAsync();
            }
        }
        
        return new JsonResult("User created");
    }

    public static async Task<JsonResult> UpdateUser(string jsonString)
    {
        var update = JsonSerializer.Deserialize<UserDb>(jsonString);
        var table = new DataTable();
        await using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            await using (var command = new NpgsqlCommand("UPDATE users SET login = @login, password = @password premium = @premium WHERE id = @id", connection))
            {
                command.Parameters.AddWithValue("id", update.Id);
                command.Parameters.AddWithValue("login", update.Login);
                command.Parameters.AddWithValue("password", update.Password);
                command.Parameters.AddWithValue("premium", update.Premium);
                
                var reader = await command.ExecuteReaderAsync();
                table.Load(reader);
                
                await reader.CloseAsync();
                await connection.CloseAsync();
            }
        }
        
        return new JsonResult("User updated");
    }

    public static async Task<JsonResult> RemoveUser(int id)
    {
        var table = new DataTable();
        await using (var connection = new NpgsqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            await using (var command = new NpgsqlCommand("DELETE FROM users WHERE id = @id", connection))
            {
                command.Parameters.AddWithValue("id", id);
                
                var reader = await command.ExecuteReaderAsync();
                table.Load(reader);
                
                await reader.CloseAsync();
                await connection.CloseAsync();
            }
        }
        
        return new JsonResult("User deleted");
    }
}