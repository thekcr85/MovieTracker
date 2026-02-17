using MovieTracker.Domain.Entities;

namespace MovieTracker.Domain.Interfaces;

public interface IMovieService
{
	Task<List<WatchedMovie>> GetWatchedMoviesAsync();
	Task<List<WatchlistMovie>> GetWatchlistMoviesAsync();
	Task<WatchedMovie?> GetWatchedMovieByIdAsync(int id);
	Task AddToWatchedAsync(WatchedMovie movie);
	Task AddToWatchlistAsync(WatchlistMovie movie);
	Task MoveToWatchedAsync(int watchlistMovieId);
	Task RemoveFromWatchlistAsync(int id);
	Task<Review?> GetReviewByMovieIdAsync(int watchedMovieId);
	Task SaveReviewAsync(Review review);
}