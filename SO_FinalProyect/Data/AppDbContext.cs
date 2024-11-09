using Microsoft.EntityFrameworkCore;
using SO_API_REST.Domain.Entities;

namespace SO_API_REST.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<House> Houses { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Entry> Entries { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Additional relationship configurations if needed
    }
}