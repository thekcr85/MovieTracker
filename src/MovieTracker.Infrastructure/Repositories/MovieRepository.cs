using Microsoft.EntityFrameworkCore;
using MovieTracker.Domain.Entities;
using MovieTracker.Domain.Interfaces;
using MovieTracker.Infrastructure.Data;

namespace MovieTracker.Infrastructure.Repositories;

public class MovieRepository : IMovieRepository
{
	private readonly ApplicationDbContext _context;

	public MovieRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<List<WatchedMovie>> GetWatchedMoviesAsync()
	{
		return await _context.WatchedMovies
			.Include(m => m.Review)
			.OrderByDescending(m => m.WatchedDate)
			.ToListAsync();
	}

	public async Task<List<WatchlistMovie>> GetWatchlistMoviesAsync()
	{
		return await _context.WatchlistMovies
			.OrderByDescending(m => m.AddedDate)
			.ToListAsync();
	}

	public async Task<WatchedMovie?> GetWatchedMovieByIdAsync(int id)
	{
		return await _context.WatchedMovies
			.Include(m => m.Review)
			.FirstOrDefaultAsync(m => m.Id == id);
	}

	public async Task AddWatchedMovieAsync(WatchedMovie movie)
	{
		_context.WatchedMovies.Add(movie);
		await _context.SaveChangesAsync();
	}

	public async Task AddWatchlistMovieAsync(WatchlistMovie movie)
	{
		_context.WatchlistMovies.Add(movie);
		await _context.SaveChangesAsync();
	}

	public async Task RemoveWatchlistMovieAsync(int id)
	{
		var movie = await _context.WatchlistMovies.FindAsync(id);
		if (movie != null)
		{
			_context.WatchlistMovies.Remove(movie);
			await _context.SaveChangesAsync();
		}
	}

	public async Task<Review?> GetReviewByWatchedMovieIdAsync(int watchedMovieId)
	{
		return await _context.Reviews
			.FirstOrDefaultAsync(r => r.WatchedMovieId == watchedMovieId);
	}

	public async Task SaveReviewAsync(Review review)
	{
		var existingReview = await _context.Reviews
			.FirstOrDefaultAsync(r => r.WatchedMovieId == review.WatchedMovieId);

		if (existingReview != null)
		{
			existingReview.Comment = review.Comment;
			existingReview.UserRating = review.UserRating;
			existingReview.UpdatedDate = DateTime.UtcNow;
			_context.Reviews.Update(existingReview);
		}
		else
		{
			_context.Reviews.Add(review);
		}

		await _context.SaveChangesAsync();
	}
}