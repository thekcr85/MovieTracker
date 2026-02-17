using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieTracker.Domain.Interfaces;
using MovieTracker.Infrastructure.Data;
using MovieTracker.Infrastructure.Repositories;
using MovieTracker.Infrastructure.Services;

namespace MovieTracker.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		// Database
		var connectionString = configuration.GetConnectionString("DefaultConnection");
		services.AddDbContext<ApplicationDbContext>(options =>
			options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 21))));

		// Repositories
		services.AddScoped<IMovieRepository, MovieRepository>();

		// External Services
		services.AddHttpClient<ITmdbService, TmdbService>();

		return services;
	}
}