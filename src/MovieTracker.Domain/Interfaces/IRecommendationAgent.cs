using MovieTracker.Domain.Entities;

namespace MovieTracker.Domain.Interfaces;

public interface IRecommendationAgent
{
	Task<List<Movie>> GetPersonalizedRecommendationsAsync(List<string> watchedMovieTitles, int count = 3);
}