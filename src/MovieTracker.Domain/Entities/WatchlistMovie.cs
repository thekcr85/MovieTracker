namespace MovieTracker.Domain.Entities;

public class WatchlistMovie
{
	public int Id { get; set; }
	public int TmdbId { get; set; }
	public string Title { get; set; } = string.Empty;
	public string? PosterPath { get; set; }
	public string? Overview { get; set; }
	public DateTime? ReleaseDate { get; set; }
	public string? Director { get; set; }
	public string? Genres { get; set; }
	public double? Rating { get; set; }
	public int? Runtime { get; set; }
	public List<string>? Actors { get; set; }
	public DateTime AddedDate { get; set; }
}