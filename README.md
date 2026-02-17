# MovieTracker ??

> AI-powered movie tracking app with personalized recommendations using OpenAI GPT-4o-mini

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-Server-512BD4)](https://blazor.net/)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1)](https://www.mysql.com/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED)](https://www.docker.com/)

## What This Does

A Blazor Server application for tracking movies with AI-powered recommendations:

- ?? **TMDB Integration** - Search movies from The Movie Database
- ?? **Watchlist** - Keep track of movies you want to watch
- ? **Watched List** - Record movies you've seen with ratings and reviews
- ?? **AI Recommendations** - Get personalized movie suggestions powered by OpenAI
- ?? Smart movie details with cast, director, genres, and ratings

## Tech Stack

```
.NET 9 + C# 13
Blazor Server (Interactive)
OpenAI GPT-4o-mini (AI Recommendations)
TMDB API (Movie Data)
Entity Framework Core 9 + MySQL 8 (Pomelo)
Docker + Docker Compose
Clean Architecture
```

## Quick Start

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [TMDB API Key](https://www.themoviedb.org/settings/api) (free)
- [OpenAI API Key](https://platform.openai.com/api-keys)

### Run with Docker

```bash
# 1. Clone
git clone https://github.com/yourusername/MovieTracker.git
cd MovieTracker

# 2. Create .env file with your API keys
echo MYSQL_ROOT_PASSWORD=rootpassword123 > .env
echo MYSQL_PASSWORD=moviepass >> .env
echo TMDB_API_KEY=your-tmdb-key-here >> .env
echo OPENAI_API_KEY=sk-your-openai-key-here >> .env

# 3. Start
docker compose up

# 4. Open browser
# ? http://localhost:8080
```

That's it! The app will:
- Start MySQL 8.0 database
- Run migrations automatically
- Launch Blazor app on port 8080

## Project Structure

```
src/
??? MovieTracker.Domain/              # ?? Core (Entities, Interfaces)
??? MovieTracker.Application/         # ?? Business Logic
??? MovieTracker.Infrastructure/      # ?? Data + External APIs
??? MovieTracker.Web/                 # ?? Blazor UI
    ??? Components/Pages/
        ??? Home.razor              # Search & AI Recommendations
        ??? Watchlist.razor         # Movies to watch
        ??? Watched.razor           # Watched movies with reviews
```

**Clean Architecture** - dependencies flow inward (Web ? Infra ? App ? Domain)

## Features

### ?? Movie Search
- Real-time search powered by TMDB API
- View detailed movie information (cast, director, ratings, overview)
- High-quality movie posters

### ?? Watchlist Management
- Add movies you want to watch
- Track when you added them
- Move movies to watched when done

### ? Watched Movies
- Mark movies as watched
- Add your own ratings (1-10)
- Write reviews and comments
- Track when you watched them

### ?? AI Recommendations
- Personalized suggestions based on your watched movies
- Powered by OpenAI GPT-4o-mini
- Smart recommendations considering your preferences

## Configuration

### Environment Variables (.env)

```bash
MYSQL_ROOT_PASSWORD=rootpassword123
MYSQL_PASSWORD=moviepass
TMDB_API_KEY=your-tmdb-api-key-here
OPENAI_API_KEY=sk-your-openai-api-key-here
```

## NuGet Packages

### MovieTracker.Infrastructure

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.0" />
<PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="9.0.0" />
<PackageReference Include="OpenAI" Version="2.1.0" />
```

**All packages are production-ready and compatible with .NET 9!**

## Docker Commands

```bash
# Start services
docker compose up

# Run in background
docker compose up -d

# View logs
docker compose logs -f web

# Stop & remove
docker compose down -v

# Rebuild
docker compose build --no-cache
```

## How to Get API Keys

### TMDB API Key (Free)
1. Go to [themoviedb.org](https://www.themoviedb.org/)
2. Create a free account
3. Go to Settings ? API
4. Request an API key (choose Developer)
5. Copy your API key

### OpenAI API Key
1. Go to [platform.openai.com](https://platform.openai.com/)
2. Sign up or log in
3. Go to API Keys section
4. Create new secret key
5. Copy your key (starts with `sk-`)

## Architecture Highlights

? **Clean Architecture** - testable, maintainable  
? **Blazor Server** - real-time interactive UI  
? **EF Core 9** - modern ORM with MySQL 8 (Pomelo)  
? **TMDB Integration** - comprehensive movie database  
? **OpenAI Recommendations** - AI-powered suggestions  
? **Docker** - one-command deployment

## Technology Details

- **Framework**: .NET 9 with C# 13
- **UI**: Blazor Server with Bootstrap 5 (Darkly theme)
- **Database**: MySQL 8.0 with EF Core 9 (Pomelo provider)
- **AI**: OpenAI GPT-4o-mini for recommendations
- **Movie Data**: TMDB API v3
- **Architecture**: Clean Architecture pattern
- **Deployment**: Docker & Docker Compose

## Author

**Your Name** • [GitHub](https://github.com/yourusername)

Project demonstrating:
- **Clean Architecture** with proper layer separation
- **Blazor Server** with interactive components (.NET 9)
- **AI Integration** using OpenAI for personalized recommendations
- **External API Integration** (TMDB)
- **Docker containerization** for one-command deployment
- **Entity Framework Core 9** with MySQL 8 (Pomelo provider)

---

## License

MIT License - Personal project

---

**Get Started:** `docker compose up` ??

