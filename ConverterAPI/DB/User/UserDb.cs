using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ConverterAPI.DB.Session;

namespace ConverterAPI.DB.User;

public static class UserDb
{
    private static readonly string ConnectionString = ConfigurationHelper.GetConnectionString("DefaultConnection");

    // Connection to DB
    private static readonly DbContextOptions<UserDbContext> Options = new DbContextOptionsBuilder<UserDbContext>()
        .UseNpgsql(ConnectionString)
        .Options;

    public static async Task<List<User>> GetUsers()
    {
        await using var context = new UserDbContext(Options);
        return await context.Users.ToListAsync();
    }

    public static async Task<User?> GetUserById(int id)
    {
        await using var context = new UserDbContext(Options);
        return await context.Users.FindAsync(id);
    }

    public static async Task<JsonResult> CreateUser(NewUser? newUser)
    {
        var user = new User();

        if (newUser != null)
        {
            user.Login = newUser.Login;
            user.Password = PasswordManager.HashPassword(newUser.Password);
            user.Premium = false;

            await using var context = new UserDbContext(Options);
            context.Users.Add(user);
            await context.SaveChangesAsync();

            return new JsonResult("User successfully created");
        }
        else return new JsonResult("User not created");
    }

    public static async Task<JsonResult> UpdateUser(User? updatedUser)
    {
        await using var context = new UserDbContext(Options);
        if (updatedUser != null)
        {
            var user = await context.Users.FindAsync(updatedUser.Id);

            if (user != null)
            {
                user.Login = updatedUser.Login ?? user.Login;
                user.Password = (updatedUser.Password != null) ? PasswordManager.HashPassword(updatedUser.Password) : user.Password;
                user.Premium = updatedUser.Premium;

                context.Users.Update(user);
                await context.SaveChangesAsync();

                return new JsonResult("User updated");
            }
            else return new JsonResult("User not found");
        }
        else return new JsonResult("Incorrect query");
    }

    public static async Task<JsonResult> DeleteUser(int id)
    {
        await using var context = new UserDbContext(Options);
        var user = await context.Users.FindAsync(id);
        if (user == null)
        {
            return new JsonResult("User not found");
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync();

        return new JsonResult("User successfully deleted");
    }
}