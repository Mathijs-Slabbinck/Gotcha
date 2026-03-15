<!-- This file is for PROJECT-SPECIFIC info only — architecture, entities, services, patterns, key paths. Global rules (code style, prompt logging) live in ~/CLAUDE.md. -->

# CLAUDE.md

**CONVENTIONS** — Read before writing code:
- `.claude/conventions/CLAUDE_GIT.md`
- `.claude/conventions/CLAUDE_CS.md`
- `.claude/conventions/CLAUDE_CSS.md`
- `.claude/conventions/CLAUDE_HTML.md`
- `.claude/conventions/CLAUDE_JAVASCRIPT.md`
- `.claude/conventions/CLAUDE_DOTNET.md`
- `.claude/conventions/CLAUDE_MVC_WEB-BACKEND.md`
- `.claude/conventions/CLAUDE_API.md`
- `.claude/conventions/CLAUDE_MAUI.md`
- `.claude/conventions/CLAUDE_TESTS.md`

**REGULATIONS** — Legal/compliance rules to follow:
- `.claude/regulations/data-privacy-laws-default.md`
- `.claude/regulations/accessibility-default.md`

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Subproject Guides

Before working in a subproject, read its guide file first:

|     Subproject     |              Guide File              |         Type          |
| :----------------: | :----------------------------------: | :-------------------: |
|    Gotcha.Core     |    `Gotcha.Core/CLAUDE-CORE.md`      |    Class Library      |
| Gotcha.Core.Tests  | `Gotcha.Core.Tests/CLAUDE-CORE-TESTS.md` |    xUnit Tests    |
|    Gotcha.Web      |    `Gotcha.Web/CLAUDE-WEB.md`        |  ASP.NET Core MVC     |
|    Gotcha.Maui     |    `Gotcha.Maui/CLAUDE-MAUI.md`      |     .NET MAUI         |
|    Gotcha.API      |    `Gotcha.API/CLAUDE-API.md`        | ASP.NET Core Web API  |

## Build & Run Commands

```bash
# Restore dependencies
dotnet restore

# Build the entire solution
dotnet build

# Run the web application (from repo root)
dotnet run --project Gotcha.Web

# Run the API (for MAUI app)
dotnet run --project Gotcha.API --launch-profile http

# Run unit tests
dotnet test Gotcha.Core.Tests

# Add an EF Core migration
dotnet ef migrations add <MigrationName> --project Gotcha.Core --startup-project Gotcha.Web

# Apply migrations
dotnet ef database update --project Gotcha.Core --startup-project Gotcha.Web
```

## Architecture

**Five-project solution** targeting .NET 10.0 with EF Core 10 + SQL Server Express (local):

- **Gotcha.Core** — Class library containing all domain logic, entities, services, and data access. Nullable reference types enabled.
- **Gotcha.Core.Tests** — xUnit test project for unit testing Core. References Gotcha.Core with `InternalsVisibleTo`.
- **Gotcha.Web** — ASP.NET Core MVC app (Razor views, Bootstrap 5, jQuery). Nullable reference types enabled. References Gotcha.Core.
- **Gotcha.Maui** — .NET MAUI mobile app (Android/iOS/Windows). Uses CommunityToolkit.Mvvm and CommunityToolkit.Maui.
- **Gotcha.API** — Internal ASP.NET Core Web API. References Gotcha.Core. Serves data to the MAUI app via REST endpoints.

### MVC Areas

The web app uses three routing contexts:

| Route prefix | Area | Purpose |
|---|---|---|
| `/` | (none) | Public pages: login, signup, info, contact, password reset |
| `/User/` | User | Authenticated user dashboard, games, settings, store |
| `/Player/` | Player | Player-specific game views: home, confirm kill, settings, admin |

Each area has its own `_Layout.cshtml`. The Player area uses partials for alive/dead navbars.

### Key Paths

- Entities: `Gotcha.Core/Entities/Models/` (GotchaUser, Player, Game, Kill, Rules, TargetAssignment, VipSettings)
- Logging: `Gotcha.Core/Entities/Logging/` (Log, Attacker, Error, Warning, HackAttempt, OtherLogType)
- Services: `Gotcha.Core/Services/` (GameService + Repository/ + ValidationServices/ + ResultModel/)
- DbContext: `Gotcha.Core/Data/GotchaDbContext.cs` (extends `IdentityDbContext`)
- Seeder: `Gotcha.Core/Data/Seeder/Seeder.cs`
- Enums: `Gotcha.Core/Enums/` (Genders, PlayerStatus, AssignmentStatus, LogTypes, LogSubTypes, Plan, MaxLobbySize)
- Exceptions: `Gotcha.Core/Exceptions/` (GotchaException base + specific types + NotFound/)
- CSS/JS: `Gotcha.Web/wwwroot/css/` and `Gotcha.Web/wwwroot/js/`
- Player ViewModels: `Gotcha.Web/Areas/Player/ViewModels/` (+ BaseViewModels/)
- User ViewModels: `Gotcha.Web/Areas/User/ViewModels/` (GameItemViewModel, GamesViewModel, NewGameViewModel, StoreViewModel)
- API DTOs: `Gotcha.API/Dtos/` (Attackers/, Games/, GotchaUsers/, Kills/, Logs/, Players/, Rules/, TargetAssignments/, VipSettings/) — includes composite DTOs (UserProfileResponseDto, GameItemResponseDto, PlayerHomeResponseDto, ConfirmKillResponseDto, AdminDataResponseDto)
- API Controllers: `Gotcha.API/Controllers/` (one per entity: Attackers, Games, GotchaUsers, Kills, Logs, Players, Rules, TargetAssignments, VipSettings) — GotchaUsers and Players have composite endpoints
- API Constants: `Gotcha.API/Constants/DevConstants.cs` (TestUserId linked to Seeder)
- Tests: `Gotcha.Core.Tests/Services/` and `Gotcha.Core.Tests/Entities/`
- Data Annotations: `Gotcha.Core/Validation/DataAnnotations/` (BirthDay, GuardianRequired, Name, Picture, UserName, UserNameOrEmail)
- MAUI Pages: `Gotcha.Maui/Pages/` (Unauthenticated/ and Authenticated/User/ + Player/)
- MAUI ViewModels: `Gotcha.Maui/ViewModels/`
- MAUI Services: `Gotcha.Maui/Services/` (interfaces) + `Services/Api/` (API implementations) + `Services/Mock/` (mock implementations)
- MAUI Models: `Gotcha.Maui/Models/` (UserProfile, GameItem, PlayerHomeData, ConfirmKillData, AdminData, StoreState, etc.)
- MAUI Enums: `Gotcha.Maui/Enums/` (Plan)
- MAUI Routes: `Gotcha.Maui/Routes.cs` (all Shell route constants)
- MAUI Constants: `Gotcha.Maui/Constants/DevConstants.cs` (TestUserId matching Seeder)
- MAUI Fonts: `Gotcha.Maui/Resources/Fonts/`
- MAUI Images: `Gotcha.Maui/Resources/Images/`
- Shared fonts (source): `fonts/` (Nosifer, Bungee, Roboto, Roboto Slab — downloaded from Google Fonts)

### Domain Model

Gotcha is an assassination-style social game platform. Core entity relationships:

- **GotchaUser** → owns many **Player** instances (one per game joined)
- **Game** → has many Players, Kills, Rules, and TargetAssignments
- **Player** → linked to a GotchaUser and a Game; tracks alive/dead status, admin and spectator flags
- **TargetAssignment** → links a Hunter (Player) to a Target (Player) with status (Ongoing/Killed/Failed/Cancelled/Revoked)
- **Kill** → records killer, victim, weapon, validity, and timestamps
- **Rules** — per-game config: game mode (Gotcha/Assassin), visibility settings (ShowPlayerImages, ShowGender, EnforcePlayerImages), timing, chaos mode, custom kill methods

Games support two target assignment strategies: circular and random. User plans (Standard/Premium/Deluxe) limit max players per game (50/100/1000).

### Services & Patterns

- **Repository pattern**: `IRepositoryService<T>` interface with concrete implementations per entity in `Gotcha.Core/Services/Repository/`. CRUD only, no business logic.
- **Service layer**: `GameService` handles orchestration (JoinPlayer, StartGame, HandleValidKill, etc.).
- **Result model pattern**: `ResultModel<T>` / `BaseResultModel` for returning data with error/warning collections.
- **Validation**: Split into 3 focused static services — `UserValidationService` (email, username, name, birthday), `ImageValidationService` (image URL, file extension, MIME type, file size, magic bytes, re-encoding), `SecurityValidationService` (IP validation). Reserved usernames = simple static HashSet, no config/DI.
- **Custom exception hierarchy**: Base `GotchaException` with specific subtypes (`GameStateException`, `GameRuleViolationException`, `ValidationException`, etc.) and per-entity `NotFoundException` classes.
- **Logging**: Custom `Log` entity system with `Attacker` tracking (IP, user agent, path) and subtypes (Error, Warning, HackAttempt).
- **DI registration**: All 7 RepoServices and GameService registered as Scoped in both `Gotcha.Web/Program.cs` and `Gotcha.API/Program.cs`. Identity services registered via `AddIdentity<IdentityUser, IdentityRole>` with `AddEntityFrameworkStores<GotchaDbContext>` (Web only).
- **Honeypot page**: `GotchaController` logs unauthorized direct navigation attempts (tracks Attacker info including session ID) and redirects to Contact with TempData pre-fill. Uses `_GotchaLayout.cshtml`.

### Entity Conventions

- POCOs with no validation in property setters, no business logic
- Auto-properties with initializers, no backing fields unless needed
- `{ get; init; }` for IDs and timestamps (locked after creation)
- No constructors on entities — use object initializer syntax
- TimeSpan stored as ticks in DB (configured in OnModelCreating)
- Cascade behavior: Game→Players (Cascade), Player→GotchaUser (Restrict), Kill→Killer/Victim (Restrict)

### Identity

- ASP.NET Identity configured in `Program.cs` with `AddIdentity<IdentityUser, IdentityRole>`
- `GotchaDbContext` extends `IdentityDbContext` (adds Identity tables: AspNetUsers, AspNetRoles, etc.)
- Password policy: min 12 chars, requires digit, lowercase, uppercase, special char, 4 unique chars
- `app.UseAuthentication()` is called before `app.UseAuthorization()` in the middleware pipeline
- Identity tables exist in DB (migration applied), but not yet fully wired (no login/register controllers, no `[Authorize]` attributes)

### Session

- Session middleware configured in `Program.cs` with `AddDistributedMemoryCache()` + `AddSession()`
- `app.UseSession()` placed after `UseRouting()`, before `UseAuthentication()`
- Cookie settings: `HttpOnly = true`, `IsEssential = true`, `SecurePolicy = Always`, 30-min idle timeout
- In-memory cache backing store (suitable for dev/single-server; swap to Redis/SQL for production)

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

