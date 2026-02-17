using Microsoft.EntityFrameworkCore;
using MovieTracker.Domain.Entities;
using System.Reflection.Emit;

namespace MovieTracker.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

	public DbSet<WatchedMovie> WatchedMovies => Set<WatchedMovie>();
	public DbSet<WatchlistMovie> WatchlistMovies => Set<WatchlistMovie>();
	public DbSet<Review> Reviews => Set<Review>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
	}
}