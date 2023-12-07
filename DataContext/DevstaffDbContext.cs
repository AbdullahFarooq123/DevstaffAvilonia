using Microsoft.EntityFrameworkCore;

namespace DataContext;

public class DevstaffDbContext : DbContext
{
	public DbSet<IntervalEntry> IntervalEntries { get; set; }
	public DbSet<Project> Projects { get; set; }
	public DbSet<Screenshot> Screenshots { get; set; }
	public DbSet<User> Users { get; set; }
	public DbSet<UserActivity> UserActivities { get; set; }

	public DevstaffDbContext(DbContextOptions<DevstaffDbContext> options)
		: base(options ?? throw new ArgumentNullException(nameof(options))) { }
}