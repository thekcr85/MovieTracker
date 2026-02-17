using MovieTracker.Domain.Entities;

namespace MovieTracker.Domain.Interfaces;

public interface ITmdbService
{
	Task<List<Movie>> SearchMoviesAsync(string query);
	Task<Movie?> GetMovieDetailsAsync(int tmdbId);
	Task<List<Movie>> GetRecommendedMoviesAsync(int count = 3);
}