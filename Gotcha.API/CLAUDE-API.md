# Gotcha.API Conventions

## Conventions

Before writing code in this subproject, read these convention files from `.claude/conventions/`:
- `CLAUDE_CS.md` — C# code style, naming, OOP, LINQ
- `CLAUDE_DOTNET.md` — Shared .NET: DI, async, ResultModel, HttpClient
- `CLAUDE_API.md` — Web API: REST, DTOs, JWT, CORS
- `CLAUDE_GIT.md` — Commit conventions, Git Flow, branching

## Project Overview

Internal ASP.NET Core Web API serving data to the MAUI app. References Gotcha.Core for entities, services, and DbContext. Uses OpenApi.

## Tech Stack

- .NET 10.0, ASP.NET Core Web API
- Microsoft.AspNetCore.OpenApi
- Microsoft.EntityFrameworkCore.SqlServer (via Gotcha.Core reference)

## Configuration

- **Port**: `http://localhost:5208` (launch profile `http`)
- **CORS**: AllowAll policy (dev only — AllowAnyOrigin/Method/Header)
- **JSON**: `ReferenceHandler.IgnoreCycles` to prevent circular reference errors
- **DB**: Same SQL Server Express connection string as Gotcha.Web
- **Seeder**: Runs on startup via `Seeder.SeedAsync()` in `Program.cs`
- **DI**: All 7 repo services + GameService registered as Scoped (mirrors Web)

## DTO Structure

DTOs organized per entity in `Dtos/` subfolders. Each entity typically has:
- `CreateXxxDto` — for POST requests
- `UpdateXxxDto` — for PUT/PATCH requests
- `XxxResponseDto` — for GET responses

|          Folder           |                                              DTOs                                              |
| :-----------------------: | :--------------------------------------------------------------------------------------------: |
|    `Dtos/Attackers/`      |                          AttackerResponseDto, CreateAttackerDto                                 |
|      `Dtos/Games/`        |             GameResponseDto, GameItemResponseDto, CreateGameDto, UpdateGameDto                  |
|   `Dtos/GotchaUsers/`    |          GotchaUserResponseDto, UserProfileResponseDto, CreateGotchaUserDto, UpdateGotchaUserDto |
|      `Dtos/Kills/`        |                             KillResponseDto, CreateKillDto                                     |
|      `Dtos/Logs/`         |                              LogResponseDto, CreateLogDto                                      |
|     `Dtos/Players/`       | PlayerResponseDto, PlayerHomeResponseDto, ConfirmKillResponseDto, AdminDataResponseDto, CreatePlayerDto, UpdatePlayerDto |
|      `Dtos/Rules/`        |                            RulesResponseDto, UpdateRulesDto                                    |
| `Dtos/TargetAssignments/` |            TargetAssignmentResponseDto, CreateTargetAssignmentDto, UpdateTargetAssignmentDto     |
|   `Dtos/VipSettings/`     |                      VipSettingsResponseDto, UpdateVipSettingsDto                              |

### Composite DTOs

These aggregate data from multiple tables for MAUI page consumption:
- `UserProfileResponseDto` — user data + stats (gamesPlayed, wins, kills, streak)
- `GameItemResponseDto` — simplified game for list views (with winner name, player count)
- `PlayerHomeResponseDto` — player + game + rules + target + hunter + kills + players (includes nested `KillItemDto`, `PlayerItemDto`)
- `ConfirmKillResponseDto` — target + hunter info for kill confirmation page
- `AdminDataResponseDto` — game + rules + players + pending kills + VIP unlocks (includes nested `AdminPlayerItemDto`, `AdminKillItemDto`)

## Controllers

One controller per entity in `Controllers/`. All use `[Route("api/[controller]")]` and `[ApiController]`.

### Route Constraints

All `{id}` and `{userId}` route parameters use `{id:guid}` / `{userId:guid}` constraints. This rejects non-GUID values at the routing level (404) instead of failing at model binding.

### Standard CRUD Endpoints

|       Controller        |              Endpoints               |
| :---------------------: | :----------------------------------: |
|   AttackersController   |              GET all                 |
|     GamesController     |       GET all, GET {id}, POST        |
|  GotchaUsersController  |     GET all, GET {id}, PUT {id}      |
|     KillsController     |              GET all                 |
|      LogsController     |               POST                   |
|    PlayersController    |   GET all, GET {id}, PATCH {id}      |
|     RulesController     |             GET {id}                 |
| TargetAssignmentsController |          GET all                 |
|  VipSettingsController  |   GET {userId}, PATCH {userId}       |

### Composite Endpoints

These use `GotchaDbContext` directly for eager loading (intentional — avoids modifying Core):

|                 Endpoint                  |      Response DTO       |         Used by          |
| :---------------------------------------: | :---------------------: | :----------------------: |
|    `GET api/gotchausers/{id}/profile`      | UserProfileResponseDto  |      IUserService        |
| `GET api/gotchausers/{id}/games?status=`  | List<GameItemResponseDto> |     IGameService       |
|       `GET api/players/{id}/home`          | PlayerHomeResponseDto   |     IPlayerService       |
|    `GET api/players/{id}/confirmkill`      | ConfirmKillResponseDto  |     IPlayerService       |
|       `GET api/players/{id}/admin`         | AdminDataResponseDto    |     IPlayerService       |

## Dev Constants

`Constants/DevConstants.cs` — references `Seeder.TestUserId` (fixed GUID `aaaaaaaa-...`) for development/testing without auth.

## Key Paths

- Controllers: `Controllers/`
- DTOs: `Dtos/` (subfolders per entity)
- Constants: `Constants/DevConstants.cs`
- Config: `Program.cs` (DI, CORS, JSON, DB, seeder)
