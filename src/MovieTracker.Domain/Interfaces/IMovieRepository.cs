using MovieTracker.Domain.Entities;

namespace MovieTracker.Domain.Interfaces;

public interface IMovieRepository
{
	Task<List<WatchedMovie>> GetWatchedMoviesAsync();
	Task<List<WatchlistMovie>> GetWatchlistMoviesAsync();
	Task<WatchedMovie?> GetWatchedMovieByIdAsync(int id);
	Task AddWatchedMovieAsync(WatchedMovie movie);
	Task AddWatchlistMovieAsync(WatchlistMovie movie);
	Task RemoveWatchlistMovieAsync(int id);
	Task<Review?> GetReviewByWatchedMovieIdAsync(int watchedMovieId);
	Task SaveReviewAsync(Review review);
}