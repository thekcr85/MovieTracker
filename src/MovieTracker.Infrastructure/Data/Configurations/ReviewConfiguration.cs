using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieTracker.Domain.Entities;

namespace MovieTracker.Infrastructure.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
	public void Configure(EntityTypeBuilder<Review> builder)
	{
		builder.HasKey(r => r.Id);

		builder.Property(r => r.Comment)
			.IsRequired()
			.HasMaxLength(2000);

		builder.Property(r => r.UserRating)
			.IsRequired(false);

		builder.Property(r => r.CreatedDate)
			.IsRequired();

		builder.Property(r => r.UpdatedDate)
			.IsRequired(false);

		builder.HasIndex(r => r.WatchedMovieId)
			.IsUnique();
	}
}