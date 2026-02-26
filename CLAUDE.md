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

# Run unit tests
dotnet test Gotcha.Core.Tests

# Add an EF Core migration
dotnet ef migrations add <MigrationName> --project Gotcha.Core --startup-project Gotcha.Web

# Apply migrations
dotnet ef database update --project Gotcha.Core --startup-project Gotcha.Web
```

## Architecture

**Three-project solution** targeting .NET 10.0 with EF Core 10 + SQL Server Express (local):

- **Gotcha.Core** — Class library containing all domain logic, entities, services, and data access. Nullable reference types enabled.
- **Gotcha.Core.Tests** — xUnit test project for unit testing Core. References Gotcha.Core with `InternalsVisibleTo`.
- **Gotcha.Web** — ASP.NET Core MVC app (Razor views, Bootstrap 5, jQuery). Nullable reference types disabled. References Gotcha.Core.

### MVC Areas

The web app uses three routing contexts:

| Route prefix | Area | Purpose |
|---|---|---|
| `/` | (none) | Public pages: login, signup, info, contact, password reset |
| `/User/` | User | Authenticated user dashboard, games, settings, store |
| `/Player/` | Player | Player-specific game views: home, confirm kill, settings, admin |

Each area has its own `_Layout.cshtml`. The Player area uses partials for alive/dead navbars.

### Key Paths

- Entities: `Gotcha.Core/Entities/Models/` (User, Player, Game, Kill, Rules, TargetAssignment, VipSettings)
- Logging: `Gotcha.Core/Entities/Logging/` (Log, Attacker, Error, Warning, HackAttempt, OtherLogType)
- Services: `Gotcha.Core/Services/` (GameService + Repository/ + ValidationServices/ + ResultModel/)
- DbContext: `Gotcha.Core/Data/GotchaDbContext.cs`
- Seeder: `Gotcha.Core/Data/Seeder/Seeder.cs`
- Enums: `Gotcha.Core/Enums/` (Genders, PlayerStatus, AssignmentStatus, LogTypes, LogSubTypes, Plan, MaxLobbySize)
- Exceptions: `Gotcha.Core/Exceptions/` (GotchaException base + specific types + NotFound/)
- CSS/JS: `Gotcha.Web/wwwroot/css/` and `Gotcha.Web/wwwroot/js/`
- Player ViewModels: `Gotcha.Web/Areas/Player/ViewModels/` (+ BaseViewModels/)
- Tests: `Gotcha.Core.Tests/Services/` and `Gotcha.Core.Tests/Entities/`

### Domain Model

Gotcha is an assassination-style social game platform. Core entity relationships:

- **User** → owns many **Player** instances (one per game joined)
- **Game** → has many Players, Kills, Rules, and TargetAssignments
- **Player** → linked to a User and a Game; tracks alive/dead status, admin and spectator flags
- **TargetAssignment** → links a Hunter (Player) to a Target (Player) with status (Ongoing/Killed/Failed/Cancelled/Revoked)
- **Kill** → records killer, victim, weapon, validity, and timestamps
- **Rules** — per-game config: game mode (Gotcha/Assassin), visibility settings (ShowPlayerImages, ShowGender, EnforcePlayerImages), timing, chaos mode, custom kill methods

Games support two target assignment strategies: circular and random. User plans (Standard/Premium/Deluxe) limit max players per game (50/100/1000).

### Services & Patterns

- **Repository pattern**: `IRepositoryService<T>` interface with concrete implementations per entity in `Gotcha.Core/Services/Repository/`. CRUD only, no business logic.
- **Service layer**: `GameService` handles orchestration (JoinPlayer, StartGame, HandleValidKill, etc.).
- **Result model pattern**: `ResultModel<T>` / `BaseResultModel` for returning data with error/warning collections.
- **Validation**: `LastLineValidationService` for email, username, IP, and image URL validation. Reserved usernames configured in `appsettings.json` under `UserSettings:ReservedUsernames`.
- **Custom exception hierarchy**: Base `GotchaException` with specific subtypes (`GameStateException`, `GameRuleViolationException`, `ValidationException`, etc.) and per-entity `NotFoundException` classes.
- **Logging**: Custom `Log` entity system with `Attacker` tracking (IP, user agent, path) and subtypes (Error, Warning, HackAttempt).
- **DI registration**: All 7 RepoServices and GameService registered as Scoped in `Gotcha.Web/Program.cs`.

### Entity Conventions

- POCOs with no validation in property setters, no business logic
- Auto-properties with initializers, no backing fields unless needed
- `{ get; init; }` for IDs and timestamps (locked after creation)
- No constructors on entities — use object initializer syntax
- TimeSpan stored as ticks in DB (configured in OnModelCreating)
- Cascade behavior: Game→Players (Cascade), Player→User (Restrict), Kill→Killer/Victim (Restrict)

### Unit Testing Conventions

- xUnit test project: `Gotcha.Core.Tests`
- Test structure mirrors Core: `Services/` and `Entities/` folders
- Naming: `MethodName_Scenario_ExpectedResult`
- Comment above each test explains what to check + expected result

### Frontend

Razor views with Bootstrap 5, jQuery, jQuery Validation, and Google Fonts (Nosifer, Bungee, Roboto Slab, Roboto). CSS variables defined in `wwwroot/css/Variables.css`. Page-specific JS and CSS files live alongside the shared `Site.css`. Responsive design with breakpoints from 500px to 1700px.

### CSS Isolation Notes

- `.cshtml.css` files use attribute scoping (`[b-abc123]`)
- Elements inside nested Razor blocks (`@if`, `@switch`) may not get the scope attribute
- Use `::deep .className` to target those elements
