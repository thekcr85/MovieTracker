using MovieTracker.Domain.Entities;
using MovieTracker.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Globalization;

namespace MovieTracker.Infrastructure.Services;

public class TmdbService : ITmdbService
{
	private readonly HttpClient _httpClient;
	private readonly string _apiKey;
	private readonly string _baseUrl;
	private const string ImageBaseUrl = "https://image.tmdb.org/t/p/w500";
	private readonly JsonSerializerOptions _jsonOptions;

	public TmdbService(HttpClient httpClient, IConfiguration configuration)
	{
		_httpClient = httpClient;
		_apiKey = configuration["TMDB:ApiKey"] ?? throw new ArgumentNullException("TMDB:ApiKey");
		_baseUrl = configuration["TMDB:BaseUrl"] ?? "https://api.themoviedb.org/3";

		_jsonOptions = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			Converters = { new NullableDateTimeConverter() }
		};
	}

	public async Task<List<Movie>> SearchMoviesAsync(string query)
	{
		if (string.IsNullOrWhiteSpace(query))
			return new List<Movie>();

		var url = $"{_baseUrl}/search/movie?api_key={_apiKey}&query={Uri.EscapeDataString(query)}";
		var response = await _httpClient.GetFromJsonAsync<TmdbSearchResponse>(url, _jsonOptions);

		return response?.Results?.Select(MapToMovie).ToList() ?? new List<Movie>();
	}

	public async Task<Movie?> GetMovieDetailsAsync(int tmdbId)
	{
		var url = $"{_baseUrl}/movie/{tmdbId}?api_key={_apiKey}&append_to_response=credits";
		var response = await _httpClient.GetFromJsonAsync<TmdbMovieDetails>(url, _jsonOptions);

		if (response == null)
			return null;

		return new Movie
		{
			TmdbId = response.Id,
			Title = response.Title ?? string.Empty,
			PosterPath = !string.IsNullOrEmpty(response.PosterPath) ? $"{ImageBaseUrl}{response.PosterPath}" : null,
			Overview = response.Overview,
			ReleaseDate = response.ReleaseDate,
			Genres = string.Join(", ", response.Genres?.Select(g => g.Name) ?? Array.Empty<string>()),
			Rating = response.VoteAverage,
			Runtime = response.Runtime,
			Actors = response.Credits?.Cast?.Take(5).Select(c => c.Name).ToList() ?? new List<string>(),
			Director = response.Credits?.Crew?.FirstOrDefault(c => c.Job == "Director")?.Name
		};
	}

	public async Task<List<Movie>> GetRecommendedMoviesAsync(int count = 3)
	{
		var url = $"{_baseUrl}/movie/popular?api_key={_apiKey}&page=1";
		var response = await _httpClient.GetFromJsonAsync<TmdbSearchResponse>(url, _jsonOptions);

		return response?.Results?.Take(count).Select(MapToMovie).ToList() ?? new List<Movie>();
	}

	private Movie MapToMovie(TmdbMovieResult result)
	{
		return new Movie
		{
			TmdbId = result.Id,
			Title = result.Title ?? string.Empty,
			PosterPath = !string.IsNullOrEmpty(result.PosterPath) ? $"{ImageBaseUrl}{result.PosterPath}" : null,
			Overview = result.Overview,
			ReleaseDate = result.ReleaseDate,
			Rating = result.VoteAverage
		};
	}

	private class NullableDateTimeConverter : JsonConverter<DateTime?>
	{
		public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.Null)
				return null;

			if (reader.TokenType == JsonTokenType.String)
			{
				var dateString = reader.GetString();

				if (string.IsNullOrWhiteSpace(dateString))
					return null;

				// Try parsing with different formats
				if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
					return date;

				if (DateTime.TryParse(dateString, out var parsedDate))
					return parsedDate;

				return null;
			}

			return null;
		}

		public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
		{
			if (value.HasValue)
				writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd"));
			else
				writer.WriteNullValue();
		}
	}

	private class TmdbSearchResponse
	{
		[JsonPropertyName("results")]
		public List<TmdbMovieResult>? Results { get; set; }
	}

	private class TmdbMovieResult
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("title")]
		public string? Title { get; set; }

		[JsonPropertyName("poster_path")]
		public string? PosterPath { get; set; }

		[JsonPropertyName("overview")]
		public string? Overview { get; set; }

		[JsonPropertyName("release_date")]
		public DateTime? ReleaseDate { get; set; }

		[JsonPropertyName("vote_average")]
		public double? VoteAverage { get; set; }
	}

	private class TmdbMovieDetails
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("title")]
		public string? Title { get; set; }

		[JsonPropertyName("overview")]
		public string? Overview { get; set; }

		[JsonPropertyName("poster_path")]
		public string? PosterPath { get; set; }

		[JsonPropertyName("release_date")]
		public DateTime? ReleaseDate { get; set; }

		[JsonPropertyName("vote_average")]
		public double? VoteAverage { get; set; }

		[JsonPropertyName("runtime")]
		public int? Runtime { get; set; }

		[JsonPropertyName("genres")]
		public List<Genre>? Genres { get; set; }

		[JsonPropertyName("credits")]
		public Credits? Credits { get; set; }
	}

	private class Genre
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;
	}

	private class Credits
	{
		[JsonPropertyName("cast")]
		public List<CastMember>? Cast { get; set; }

		[JsonPropertyName("crew")]
		public List<CrewMember>? Crew { get; set; }
	}

	private class CastMember
	{
		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;
	}

	private class CrewMember
	{
		[JsonPropertyName("name")]
		public string Name { get; set; } = string.Empty;

		[JsonPropertyName("job")]
		public string Job { get; set; } = string.Empty;
	}
}
