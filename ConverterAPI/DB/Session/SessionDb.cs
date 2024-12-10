using Microsoft.EntityFrameworkCore;
using ConverterAPI.DB.User;
using Microsoft.AspNetCore.Mvc;

namespace ConverterAPI.DB.Session;

public class SessionDb
{
    private static readonly string ConnectionString = ConfigurationHelper.GetConnectionString("DefaultConnection");

    // Connection to DB
    private static readonly DbContextOptions<SessionDbContext> Options = new DbContextOptionsBuilder<SessionDbContext>()
        .UseNpgsql(ConnectionString)
        .Options;
    
    public static async Task<JsonResult> AuthUser(User.User? user)
    {
        var session = new Session();
        bool isAuthenticated = false;
        if (user != null)
        {
            await using var context = new SessionDbContext(Options);
            context.Sessions.Add(session);
            await context.SaveChangesAsync();

            return new JsonResult("Session started");
        }
        else return new JsonResult("Unable to start session");
    }
}