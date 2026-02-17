using Microsoft.Extensions.DependencyInjection;
using MovieTracker.Domain.Interfaces;
using MovieTracker.Application.Services;
using MovieTracker.Application.Agents;

namespace MovieTracker.Application;

public static class DependencyInjection
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<IMovieService, MovieService>();
		services.AddScoped<IRecommendationAgent, RecommendationAgent>();

		return services;
	}
}
