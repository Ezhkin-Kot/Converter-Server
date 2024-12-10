namespace ConverterAPI.DB.Session;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ConverterAPI.DB.User;

public class SessionDb
{
    private static readonly string ConnectionString = ConfigurationHelper.GetConnectionString("DefaultConnection");
    
    // Connection to DB
    private static readonly DbContextOptions<ApplicationDbContext> Options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseNpgsql(ConnectionString)
        .Options;
    
    public static async Task<JsonResult> AuthUser(NewUser? newUser)
    {
        var session = new Session();
        if (newUser != null)
        {
            await using var applicationDbContext = new ApplicationDbContext(Options);
            var user = await applicationDbContext.Users.FindAsync(newUser.login);
            if (user == null)
            {
                return new JsonResult(new { success = false, error = "Invalid login or password" });
            }
            if (PasswordManager.VerifyHashedPassword(user.password, newUser.password))
            {
                session.userid = user.id;
                session.datetime = DateTime.Now;
            
                await applicationDbContext.Sessions.AddAsync(session);
                await applicationDbContext.SaveChangesAsync();

                return new JsonResult("Session started");
            }
            else return new JsonResult(new { success = false, error = "Invalid login or password" });
        }
        else return new JsonResult("Incorrect query");
    }
}