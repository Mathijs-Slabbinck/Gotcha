# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run Commands

```bash
# Restore dependencies
dotnet restore

# Build the entire solution
dotnet build

# Run the web application (from repo root)
dotnet run --project Gotcha.Web

# Add an EF Core migration
dotnet ef migrations add <MigrationName> --project Gotcha.Core --startup-project Gotcha.Web

# Apply migrations
dotnet ef database update --project Gotcha.Core --startup-project Gotcha.Web
```

No test projects exist yet.

## Architecture

**Two-project solution** targeting .NET 10.0 with EF Core 10 + SQL Server:

- **Gotcha.Core** — Class library containing all domain logic, entities, services, and data access. Nullable reference types enabled.
- **Gotcha.Web** — ASP.NET Core MVC app (Razor views, Bootstrap 5, jQuery). Nullable reference types disabled. References Gotcha.Core.

### MVC Areas

The web app uses three routing contexts:

| Route prefix | Area | Purpose |
|---|---|---|
| `/` | (none) | Public pages: login, signup, info, contact, password reset |
| `/User/` | User | Authenticated user dashboard, games, settings |
| `/Player/` | Player | Player-specific game views |

### Domain Model

Gotcha is an assassination-style social game platform. Core entity relationships:

- **User** → owns many **Player** instances (one per game joined)
- **Game** → has many Players, Kills, Rules, and TargetAssignments
- **Player** → linked to a User and a Game; tracks alive/dead status
- **TargetAssignment** → links a Hunter (Player) to a Target (Player) with status (Ongoing/Killed/Failed/Cancelled/Revoked)
- **Kill** → records killer, victim, weapon, validity, and timestamps
- **Rules** — per-game config: game mode (Gotcha/Assassin), visibility settings, timing, chaos mode, custom kill methods

Games support two target assignment strategies: circular and random. User plans (Standard/Premium/Deluxe) limit max players per game (50/100/1000).

### Services & Patterns

- **Repository pattern**: `IRepositoryService<T>` interface with concrete implementations per entity (e.g., `UserRepoService`, `GameRepoService`) in `Gotcha.Core/Services/Repository/`.
- **Result model pattern**: `ResultModel<T>` / `BaseResultModel` for returning data with error/warning collections.
- **Validation**: `LastLineValidationService` for email, username, IP, and image URL validation. Reserved usernames configured in `appsettings.json` under `UserSettings:ReservedUsernames`.
- **Custom exception hierarchy**: Base `GotchaException` with specific subtypes (`GameStateException`, `GameRuleViolationException`, `ValidationException`, `InsufficientPlayersException`, etc.) and per-entity `NotFoundException` classes.
- **Logging**: Custom `Log` entity system with `Attacker` tracking (IP, user agent, path) and subtypes (Error, Warning, HackAttempt).
- **DI registration**: Services registered in `Gotcha.Web/Program.cs`. Currently only `LogRepoService` is registered as scoped — other services need registration as they're wired up.

### Frontend

Razor views with Bootstrap 5, jQuery, and Google Fonts (Nosifer, Bungee, Roboto Slab, Roboto). CSS variables defined in `wwwroot/css/Variables.css`. Page-specific JS and CSS files live alongside the shared `Site.css`. Responsive design with breakpoints from 500px to 1700px.
