namespace ConverterAPI.DB.User;

using Microsoft.EntityFrameworkCore;
using ConverterAPI.DB.Session;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Session> Sessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Session>().ToTable("sessions");
    }
}