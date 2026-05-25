using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Cooperative> Cooperatives { get; set; }
    public DbSet<CooperativeMember> CooperativeMembers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Role>()
            .HasIndex(r => r.Name)
            .IsUnique();

        modelBuilder.Entity<Cooperative>()
            .HasIndex(c => c.Name)
            .IsUnique();

        modelBuilder.Entity<Cooperative>()
            .HasOne(c => c.CreatedByUser)
            .WithMany(u => u.CreatedCooperatives)
            .HasForeignKey(c => c.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cooperative>()
            .HasOne(c => c.ReviewedByUser)
            .WithMany(u => u.ReviewedCooperatives)
            .HasForeignKey(c => c.ReviewedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
