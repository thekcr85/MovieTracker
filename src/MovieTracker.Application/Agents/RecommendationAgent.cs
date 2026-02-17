using Microsoft.Extensions.Configuration;
using MovieTracker.Domain.Entities;
using MovieTracker.Domain.Interfaces;
using OpenAI.Chat;
using System.Text.Json;

namespace MovieTracker.Application.Agents;

public class RecommendationAgent : IRecommendationAgent
{
	private readonly ITmdbService _tmdbService;
	private readonly string _openAiApiKey;
	private readonly string _model = "gpt-4o-mini";

	public RecommendationAgent(ITmdbService tmdbService, IConfiguration configuration)
	{
		_tmdbService = tmdbService;
		_openAiApiKey = configuration["OpenAI:ApiKey"] 
			?? throw new ArgumentNullException("OpenAI:ApiKey", "OpenAI API Key is not configured");
	}

	public async Task<List<Movie>> GetPersonalizedRecommendationsAsync(List<string> watchedMovieTitles, int count = 3)
	{
		if (!watchedMovieTitles.Any())
		{
			return await _tmdbService.GetRecommendedMoviesAsync(count);
		}

		try
		{
			var chatClient = new ChatClient(_model, _openAiApiKey);

			var systemPrompt = @"You are a movie recommendation expert. Based on the user's watched movies, 
suggest similar movies they might enjoy. Return ONLY a JSON array of movie titles, nothing else. 
Example format: [""Movie Title 1"", ""Movie Title 2"", ""Movie Title 3""]";

			var userPrompt = $@"Based on these movies I've watched: {string.Join(", ", watchedMovieTitles)}
Please recommend {count} similar movies I might enjoy. Return only a JSON array of movie titles.";

			var messages = new List<ChatMessage>
			{
				new SystemChatMessage(systemPrompt),
				new UserChatMessage(userPrompt)
			};

			var response = await chatClient.CompleteChatAsync(messages);
			var content = response.Value.Content[0].Text;

			var recommendedTitles = JsonSerializer.Deserialize<List<string>>(content.Trim()) ?? new List<string>();

			var recommendedMovies = new List<Movie>();
			foreach (var title in recommendedTitles.Take(count))
			{
				var searchResults = await _tmdbService.SearchMoviesAsync(title);
				if (searchResults.Any())
				{
					var movie = await _tmdbService.GetMovieDetailsAsync(searchResults.First().TmdbId);
					if (movie != null)
					{
						recommendedMovies.Add(movie);
					}
				}
			}

			if (recommendedMovies.Count < count)
			{
				var fallbackMovies = await _tmdbService.GetRecommendedMoviesAsync(count - recommendedMovies.Count);
				recommendedMovies.AddRange(fallbackMovies);
			}

			return recommendedMovies.Take(count).ToList();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error getting AI recommendations: {ex.Message}");
			return await _tmdbService.GetRecommendedMoviesAsync(count);
		}
	}
}
