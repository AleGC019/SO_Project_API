using Microsoft.EntityFrameworkCore;
using RM_API.Core.Entities;

namespace RM_API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<House> Houses { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Entry> Entries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply global query filters for IsActive status
        modelBuilder.Entity<User>().HasQueryFilter(u => u.IsActive);
        modelBuilder.Entity<Role>().HasQueryFilter(r => r.IsActive);
        modelBuilder.Entity<House>().HasQueryFilter(h => h.IsActive);
        modelBuilder.Entity<Permission>().HasQueryFilter(p => p.IsActive);
        modelBuilder.Entity<Entry>().HasQueryFilter(e => e.IsActive);

        // Configure Role
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(5)
                .HasConversion<string>(); // Store enum as string

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            entity.HasMany(r => r.Users)
                .WithOne(u => u.UserRole)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(10);

            entity.Property(e => e.UserEmail)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.UserPassword)
                .IsRequired();

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            entity.HasOne(u => u.UserRole)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(u => u.UserHouse)
                .WithMany(h => h.Inhabitants)
                .HasForeignKey(u => u.HouseId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure House
        modelBuilder.Entity<House>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.HouseNumber)
                .IsRequired();

            entity.Property(e => e.HouseAddress)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            entity.HasMany(h => h.Inhabitants)
                .WithOne(u => u.UserHouse)
                .HasForeignKey(u => u.HouseId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Permission
        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StartDate)
                .IsRequired();

            entity.Property(e => e.EndDate)
                .IsRequired();

            entity.Property(e => e.Status)
                .IsRequired();

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            entity.HasOne(p => p.PermissionHouse)
                .WithMany()
                .HasForeignKey(p => p.HouseId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.PermissionUser)
                .WithMany(u => u.Permissions)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configure Entry
        modelBuilder.Entity<Entry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EntryComment)
                .HasMaxLength(50);

            entity.Property(e => e.EntryTimestamp)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()"); // Default to UTC timestamp

            entity.Property(e => e.EntryTerminal)
                .IsRequired()
                .HasConversion<string>(); // Store enum as string

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            entity.HasOne(e => e.EntryPermission)
                .WithMany()
                .HasForeignKey(e => e.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}