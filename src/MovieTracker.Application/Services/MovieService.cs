using MovieTracker.Domain.Entities;
using MovieTracker.Domain.Interfaces;

namespace MovieTracker.Application.Services;

public class MovieService : IMovieService
{
	private readonly IMovieRepository _repository;

	public MovieService(IMovieRepository repository)
	{
		_repository = repository;
	}

	public async Task<List<WatchedMovie>> GetWatchedMoviesAsync()
	{
		return await _repository.GetWatchedMoviesAsync();
	}

	public async Task<List<WatchlistMovie>> GetWatchlistMoviesAsync()
	{
		return await _repository.GetWatchlistMoviesAsync();
	}

	public async Task<WatchedMovie?> GetWatchedMovieByIdAsync(int id)
	{
		return await _repository.GetWatchedMovieByIdAsync(id);
	}

	public async Task AddToWatchedAsync(WatchedMovie movie)
	{
		movie.WatchedDate = DateTime.UtcNow;
		await _repository.AddWatchedMovieAsync(movie);
	}

	public async Task AddToWatchlistAsync(WatchlistMovie movie)
	{
		movie.AddedDate = DateTime.UtcNow;
		await _repository.AddWatchlistMovieAsync(movie);
	}

	public async Task MoveToWatchedAsync(int watchlistMovieId)
	{
		var watchlistMovies = await _repository.GetWatchlistMoviesAsync();
		var watchlistMovie = watchlistMovies.FirstOrDefault(m => m.Id == watchlistMovieId);

		if (watchlistMovie != null)
		{
			var watchedMovie = new WatchedMovie
			{
				TmdbId = watchlistMovie.TmdbId,
				Title = watchlistMovie.Title,
				PosterPath = watchlistMovie.PosterPath,
				Overview = watchlistMovie.Overview,
				ReleaseDate = watchlistMovie.ReleaseDate,
				Director = watchlistMovie.Director,
				Genres = watchlistMovie.Genres,
				Rating = watchlistMovie.Rating,
				Runtime = watchlistMovie.Runtime,
				WatchedDate = DateTime.UtcNow
			};

			await _repository.AddWatchedMovieAsync(watchedMovie);
			await _repository.RemoveWatchlistMovieAsync(watchlistMovieId);
		}
	}

	public async Task RemoveFromWatchlistAsync(int id)
	{
		await _repository.RemoveWatchlistMovieAsync(id);
	}

	public async Task<Review?> GetReviewByMovieIdAsync(int watchedMovieId)
	{
		return await _repository.GetReviewByWatchedMovieIdAsync(watchedMovieId);
	}

	public async Task SaveReviewAsync(Review review)
	{
		review.CreatedDate = DateTime.UtcNow;
		await _repository.SaveReviewAsync(review);
	}
}
