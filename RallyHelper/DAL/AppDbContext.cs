using Microsoft.EntityFrameworkCore;
using Domain;

namespace DAL;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Define DB sets for your entities
    public DbSet<Item> Items { get; set; } = null!;
    public DbSet<ItemCategory> ItemCategories { get; set; } = null!;
    public DbSet<ItemLocation> ItemLocations { get; set; } = null!;
    public DbSet<Job> Jobs { get; set; } = null!;
    public DbSet<JobItem> JobItems { get; set; } = null!;
    public DbSet<Rally> Rallies { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Example configurations - adjust as needed

        // Unique constraint for Item
        modelBuilder.Entity<Item>()
            .HasIndex(i => i.Id)
            .IsUnique();

        // Configuring one-to-many relationships
        modelBuilder.Entity<Item>()
            .HasOne(i => i.Category)
            .WithMany(c => c.Items)
            .HasForeignKey(i => i.CategoryId);

        modelBuilder.Entity<Item>()
            .HasOne(i => i.Location)
            .WithMany(l => l.Items)
            .HasForeignKey(i => i.LocationId);

        modelBuilder.Entity<JobItem>()
            .HasKey(ji => new { ji.JobId, ji.ItemId });

        modelBuilder.Entity<JobItem>()
            .HasOne(ji => ji.Job)
            .WithMany(j => j.JobItems)
            .HasForeignKey(ji => ji.JobId);

        modelBuilder.Entity<JobItem>()
            .HasOne(ji => ji.Item)
            .WithMany() // If not tracking back to JobItems
            .HasForeignKey(ji => ji.ItemId);

        // Configure Rallies and Jobs relationship
        modelBuilder.Entity<Rally>()
            .HasMany(r => r.Jobs)
            .WithOne()
            .HasForeignKey(j => j.RallyId); // assuming RallyId is a foreign key in Jobs
    }
}