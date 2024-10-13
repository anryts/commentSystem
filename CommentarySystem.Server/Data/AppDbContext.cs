using CommentarySystem.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;
using File = CommentarySystem.Server.Data.Entities.File;

namespace WebApplication1.Data;

public class AppDbContext : DbContext
{
    public DbSet<Comment> Comment { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<File> File { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSnakeCaseNamingConvention();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}