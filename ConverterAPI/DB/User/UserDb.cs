namespace ConverterAPI.DB.User;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ConverterAPI.DB.Session;

public static class UserDb
{
    private static readonly string ConnectionString = ConfigurationHelper.GetConnectionString("DefaultConnection");

    // Connection to DB
    private static readonly DbContextOptions<ApplicationDbContext> Options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseNpgsql(ConnectionString)
        .Options;

    public static async Task<List<User>> GetUsers()
    {
        await using var context = new ApplicationDbContext(Options);
        return await context.Users.ToListAsync();
    }

    public static async Task<User?> GetUserById(int id)
    {
        await using var context = new ApplicationDbContext(Options);
        return await context.Users.FindAsync(id);
    }

    public static async Task<JsonResult> CreateUser(NewUser? newUser)
    {
        if (newUser != null)
        {
            await using var context = new ApplicationDbContext(Options);
            var user = new User();
            user.login = newUser.login;
            (user.password, user.salt) = PasswordManager.HashPassword(newUser.password);
            user.premium = false;
            
            var existUser = await context.Users.FirstOrDefaultAsync(u => u.login == user.login);
            if (existUser != null)
            {
                return new JsonResult("User already exists");
            }
            
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return new JsonResult("User successfully created");
        }
        else return new JsonResult("User not created");
    }

    public static async Task<JsonResult> UpdateUser(User? updatedUser)
    {
        if (updatedUser != null)
        {
            await using var context = new ApplicationDbContext(Options);
            var user = await context.Users.FindAsync(updatedUser.id);

            if (user != null)
            {
                user.login = updatedUser.login ?? user.login;
                (user.password, user.salt) = (updatedUser.password != null) ? 
                    PasswordManager.HashPassword(updatedUser.password) : (user.password, user.salt);
                user.premium = updatedUser.premium;

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
        await using var context = new ApplicationDbContext(Options);
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