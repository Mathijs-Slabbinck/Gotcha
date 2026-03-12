# Gotcha.API Conventions

## Conventions

Before writing code in this subproject, read these convention files from `.claude/conventions/`:
- `CLAUDE_CS.md` — C# code style, naming, OOP, LINQ
- `CLAUDE_DOTNET.md` — Shared .NET: DI, async, ResultModel, HttpClient
- `CLAUDE_API.md` — Web API: REST, DTOs, JWT, CORS
- `CLAUDE_GIT.md` — Commit conventions, Git Flow, branching

## Project Overview

Internal ASP.NET Core Web API with DTOs for model entities. Not public-facing. Uses OpenApi.

## Tech Stack

- .NET 10.0, ASP.NET Core Web API
- Microsoft.AspNetCore.OpenApi

## DTO Structure

DTOs organized per entity in `Dtos/` subfolders. Each entity typically has:
- `CreateXxxDto` — for POST requests
- `UpdateXxxDto` — for PUT/PATCH requests
- `XxxResponseDto` — for GET responses

|       Folder       |                DTOs                |
| :----------------: | :--------------------------------: |
| `Dtos/Attackers/`  | AttackerResponseDto, CreateAttackerDto |
| `Dtos/Games/`      | GameResponseDto, CreateGameDto, UpdateGameDto |
| `Dtos/GotchaUsers/` | GotchaUserResponseDto, CreateGotchaUserDto, UpdateGotchaUserDto |
| `Dtos/Kills/`      | KillResponseDto, CreateKillDto     |
| `Dtos/Logs/`       | LogResponseDto, CreateLogDto       |
| `Dtos/Players/`    | PlayerResponseDto, CreatePlayerDto, UpdatePlayerDto |
| `Dtos/Rules/`      | RulesResponseDto, UpdateRulesDto   |
| `Dtos/TargetAssignments/` | TargetAssignmentResponseDto, CreateTargetAssignmentDto, UpdateTargetAssignmentDto |
| `Dtos/VipSettings/` | UpdateVipSettingsDto              |

## Controllers

One controller per entity in `Controllers/`:
Attackers, Games, GotchaUsers, Kills, Logs, Players, Rules, TargetAssignments, VipSettings

## Key Paths

- Controllers: `Controllers/`
- DTOs: `Dtos/` (subfolders per entity)
