using MovieTracker.Web.Components;
using MovieTracker.Infrastructure;
using MovieTracker.Application;
using MovieTracker.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

// Add Infrastructure and Application layers
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

// Database migration with retry logic
using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
	var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

	var maxRetries = 10;
	var delay = TimeSpan.FromSeconds(2);

	for (int i = 0; i < maxRetries; i++)
	{
		try
		{
			logger.LogInformation("Attempting to connect to database... (Attempt {Attempt}/{MaxRetries})", i + 1, maxRetries);
			dbContext.Database.EnsureCreated();
			logger.LogInformation("Database connection successful!");
			break;
		}
		catch (Exception ex)
		{
			if (i == maxRetries - 1)
			{
				logger.LogError(ex, "Failed to connect to database after {MaxRetries} attempts", maxRetries);
				throw;
			}
			logger.LogWarning("Database connection failed. Retrying in {Delay} seconds...", delay.TotalSeconds);
			await Task.Delay(delay);
		}
	}
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.Run();
