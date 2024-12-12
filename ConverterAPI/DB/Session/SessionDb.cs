namespace ConverterAPI.DB.Session;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ConverterAPI.DB.User;

public static class SessionDb
{
    private static readonly string ConnectionString = ConfigurationHelper.GetConnectionString("DefaultConnection");
    
    // Connection to DB
    private static readonly DbContextOptions<ApplicationDbContext> Options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseNpgsql(ConnectionString)
        .Options;
    
    public static async Task<JsonResult> AuthUser(NewUser? newUser)
    {
        if (newUser != null)
        {
            await using var applicationDbContext = new ApplicationDbContext(Options);
            var user = await applicationDbContext.Users.FirstOrDefaultAsync(u => u.login == newUser.login);
            if (user != null && PasswordManager.VerifyPassword(newUser.password, user.password, user.salt))
            {
                var session = new Session();
                session.userid = user.id;
                session.datetime = DateTime.UtcNow;
            
                await applicationDbContext.Sessions.AddAsync(session);
                await applicationDbContext.SaveChangesAsync();

                return new JsonResult("Session started");
            }
            else return new JsonResult(new { success = false, error = "Invalid login or password" });
        }
        else return new JsonResult("Incorrect query");
    }
    
    public static async Task<JsonResult> DeleteSession(int userid)
    {
        await using var context = new ApplicationDbContext(Options);
        var session = await context.Sessions.FirstOrDefaultAsync(s => s.userid == userid);
        if (session == null)
        {
            return new JsonResult("Session not found");
        }

        context.Sessions.Remove(session);
        await context.SaveChangesAsync();

        return new JsonResult("Session successfully deleted");
    }
}