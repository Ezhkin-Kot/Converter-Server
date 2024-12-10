using Microsoft.EntityFrameworkCore;

namespace ConverterAPI.DB.Session;

public class SessionDbContext(DbContextOptions<SessionDbContext> options) : DbContext(options)
{
    public DbSet<Session> Sessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Model settings (optional)
        modelBuilder.Entity<Session>().ToTable("sessions");
    }
}