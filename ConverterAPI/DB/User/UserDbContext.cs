using Microsoft.EntityFrameworkCore;

namespace ConverterAPI.DB.User;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Model settings (optional)
        modelBuilder.Entity<User>().ToTable("users");
    }
}