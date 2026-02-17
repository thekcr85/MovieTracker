using Microsoft.Extensions.Configuration;
using MovieTracker.Domain.Entities;
using MovieTracker.Domain.Interfaces;
using OpenAI.Chat;
using System.Text.Json;

namespace MovieTracker.Application.Agents;

/// <summary>
/// Movie recommendation agent using Microsoft Agent Framework conventions
/// Simple, clean implementation with OpenAI ChatClient
/// </summary>
public class RecommendationAgent : IRecommendationAgent
{
	private readonly ITmdbService _tmdbService;
	private readonly ChatClient _chatClient;
	private readonly string _systemPrompt;

	public RecommendationAgent(ITmdbService tmdbService, IConfiguration configuration)
	{
		_tmdbService = tmdbService;

		// Read from environment variable first, then fallback to config
		var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
			?? configuration["OpenAI:ApiKey"] 
			?? throw new ArgumentNullException("OPENAI_API_KEY", "OpenAI API Key is not configured");

		var modelName = configuration["OpenAI:ModelName"] ?? "gpt-4o-mini";

		// Initialize ChatClient for AI agent interactions
		_chatClient = new ChatClient(modelName, apiKey);

		_systemPrompt = @"You are a movie recommendation expert. Based on the user's watched movies, 
suggest similar movies they might enjoy. Return ONLY a JSON array of movie titles, nothing else. 
Example format: [""Movie Title 1"", ""Movie Title 2"", ""Movie Title 3""]";
	}

	public async Task<List<Movie>> GetPersonalizedRecommendationsAsync(List<string> watchedMovieTitles, int count = 3)
	{
		if (!watchedMovieTitles.Any())
		{
			return await _tmdbService.GetRecommendedMoviesAsync(count);
		}

		try
		{
			var userMessage = $@"Based on these movies I've watched: {string.Join(", ", watchedMovieTitles)}
Please recommend {count} similar movies I might enjoy. Return only a JSON array of movie titles.";

			// Build message list with system prompt and user request
			var messages = new List<ChatMessage>
			{
				new SystemChatMessage(_systemPrompt),
				new UserChatMessage(userMessage)
			};

			// Execute AI agent chat completion
			var response = await _chatClient.CompleteChatAsync(messages);
			var content = response.Value.Content[0].Text;

			// Parse AI response to get movie titles
			var recommendedTitles = JsonSerializer.Deserialize<List<string>>(content.Trim()) 
				?? new List<string>();

			// Fetch full movie details from TMDB for each recommended title
			var recommendations = new List<Movie>();

			foreach (var title in recommendedTitles.Take(count))
			{
				var searchResults = await _tmdbService.SearchMoviesAsync(title);
				var movie = searchResults.FirstOrDefault();
				if (movie != null)
				{
					recommendations.Add(movie);
				}
			}

			return recommendations;
		}
		catch (Exception)
		{
			// Fallback to popular movies if AI recommendation fails
			return await _tmdbService.GetRecommendedMoviesAsync(count);
		}
	}
}
