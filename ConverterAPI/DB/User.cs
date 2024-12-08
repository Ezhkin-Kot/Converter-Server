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
    private static List<UserDb> _users = new List<UserDb>();

    public static async Task<List<UserDb>> GetUsers()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("SELECT * FROM users");
        return _users;
    }

    public static async Task<UserDb?> GetUser(int id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("SELECT * FROM users WHERE id = @id");
        return _users.SingleOrDefault(user => user.Id == id);
    }

    public static async Task<UserDb> CreateUser(UserDb userDb)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("INSERT INTO users (login, password) VALUES (@login, @password)");
        _users.Add(userDb);
        return userDb;
    }

    public static async Task<UserDb> UpdateUser(UserDb update)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        _users = _users.Select(user =>
        {
            if (user.Id == update.Id)
            {
                user.Login = update.Login;
                user.Password = update.Password;
                user.Premium = update.Premium;
            }
            return user;
        }).ToList();
        return update;
    }

    public static async void RemoveUser(int id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        _users = _users.FindAll(user => user.Id != id).ToList();
    }
}