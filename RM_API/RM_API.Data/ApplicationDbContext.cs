using Microsoft.EntityFrameworkCore;
using RM_API.Core.Entities;

namespace RM_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Entry> Entries { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Global query filter for soft delete (IsActive = true)
            modelBuilder.Entity<User>().HasQueryFilter(u => u.IsActive);
            modelBuilder.Entity<House>().HasQueryFilter(h => h.IsActive);
            modelBuilder.Entity<Permission>().HasQueryFilter(p => p.IsActive);
            modelBuilder.Entity<Entry>().HasQueryFilter(e => e.IsActive);
            modelBuilder.Entity<Role>().HasQueryFilter(r => r.IsActive);

            // User entity configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UserEmail).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UserPassword).IsRequired().HasMaxLength(100);
                entity.Property(e => e.UserAge).IsRequired();

                // Relationship with Role (many-to-one)
                entity.HasOne(u => u.UserRole)
                      .WithMany(r => r.Usuarios)
                      .HasForeignKey(u => u.RoleId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relationship with House (many-to-one)
                entity.HasOne(u => u.UserHouse)
                      .WithMany(h => h.Inhabitants)
                      .HasForeignKey(u => u.HouseId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // House entity configuration
            modelBuilder.Entity<House>(entity =>
            {
                entity.HasKey(e => e.HouseId);
                entity.Property(e => e.HouseNumber).IsRequired();
                entity.Property(e => e.HouseAddress).IsRequired().HasMaxLength(100);

                // Relationship with User (one-to-many)
                entity.HasMany(h => h.Inhabitants)
                      .WithOne(u => u.UserHouse)
                      .HasForeignKey(u => u.HouseId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Permission entity configuration
            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(e => e.PermissionId);
                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.EndDate).IsRequired();
                entity.Property(e => e.Status).IsRequired();

                // Relationship with House (many-to-one)
                entity.HasOne(p => p.PermissionHouse)
                      .WithMany()
                      .HasForeignKey(p => p.HouseId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relationship with User (many-to-one)
                entity.HasOne(p => p.PermissionUser)
                      .WithMany(u => u.Permissions)
                      .HasForeignKey(p => p.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Entry entity configuration
            modelBuilder.Entity<Entry>(entity =>
            {
                entity.HasKey(e => e.EntryId);
                entity.Property(e => e.EntryComment).HasMaxLength(500);
                entity.Property(e => e.EntryTimestamp).IsRequired();
                entity.Property(e => e.EntryTerminal).IsRequired();

                // Relationship with Permission (many-to-one)
                entity.HasOne(e => e.EntryPermission)
                      .WithMany()
                      .HasForeignKey(e => e.PermissionId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Role entity configuration
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.RoleId);
                entity.Property(e => e.RoleName)
                      .IsRequired()
                      .HasConversion<string>()
                      .HasMaxLength(50);

                // Relationship with User (one-to-many)
                entity.HasMany(r => r.Usuarios)
                      .WithOne(u => u.UserRole)
                      .HasForeignKey(u => u.RoleId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Enum configuration for TerminalType in Entry entity
            modelBuilder.Entity<Entry>()
                .Property(e => e.EntryTerminal)
                .HasConversion<string>()
                .IsRequired();
        }
    }
}