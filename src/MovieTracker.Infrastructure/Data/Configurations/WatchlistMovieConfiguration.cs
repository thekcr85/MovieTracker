using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieTracker.Domain.Entities;
using System.Text.Json;

namespace MovieTracker.Infrastructure.Data.Configurations;

public class WatchlistMovieConfiguration : IEntityTypeConfiguration<WatchlistMovie>
{
	public void Configure(EntityTypeBuilder<WatchlistMovie> builder)
	{
		builder.HasKey(m => m.Id);

		builder.Property(m => m.TmdbId)
			.IsRequired();

		builder.Property(m => m.Title)
			.IsRequired()
			.HasMaxLength(500);

		builder.Property(m => m.PosterPath)
			.HasMaxLength(500);

		builder.Property(m => m.Overview)
			.HasMaxLength(2000);

		builder.Property(m => m.Director)
			.HasMaxLength(200);

		builder.Property(m => m.Genres)
			.HasMaxLength(500);

		builder.Property(m => m.Actors)
			.HasMaxLength(1000)
			.HasConversion(
				v => v == null ? null : JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
				v => v == null ? null : JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null)
			);

		builder.Property(m => m.AddedDate)
			.IsRequired();

		builder.HasIndex(m => m.TmdbId);
	}
}