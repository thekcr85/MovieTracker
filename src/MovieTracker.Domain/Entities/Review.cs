namespace MovieTracker.Domain.Entities;

public class Review
{
	public int Id { get; set; }
	public int WatchedMovieId { get; set; }
	public string Comment { get; set; } = string.Empty;
	public int? UserRating { get; set; }
	public DateTime CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }

	public WatchedMovie WatchedMovie { get; set; } = null!;
}