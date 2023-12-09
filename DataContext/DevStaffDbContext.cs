using Microsoft.EntityFrameworkCore;

namespace DataContext;

public class DevStaffDbContext : DbContext
{
    public DbSet<IntervalEntry> IntervalEntries { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<Screenshot> Screenshots { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserActivity> UserActivities { get; set; } = null!;

    public DevStaffDbContext(DbContextOptions<DevStaffDbContext> options)
        : base(options ?? throw new ArgumentNullException(nameof(options)))
    {
    }
}