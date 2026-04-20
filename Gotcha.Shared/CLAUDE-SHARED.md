# Gotcha.Shared Guide

## Purpose

A tiny class library holding types that **cross project boundaries**. Every other project (`Gotcha.Core`, `Gotcha.Web`, `Gotcha.API`, `Gotcha.Maui`) references this one. Nothing in the solution references *into* Shared's dependencies — Shared has no dependencies itself.

The point: avoid duplicating the same enum, constant, or helper in multiple projects just because one side of the HTTP boundary (MAUI) can't reference the server-side project (Core). If the exact same type needs to exist on both sides of the MAUI↔API wire, it lives here.

## What goes in Gotcha.Shared

- **Enums** that appear in API DTOs and need to be consumed the same way by MAUI.
- **Constants / GUIDs** that multiple projects must agree on (seed user IDs, fixed feature identifiers, etc.).
- **Pure helper methods** — no I/O, no external deps — that both sides need (e.g. `AgeHelper.IsUnder16` for the guardian-consent flow).

## What does NOT go in Gotcha.Shared

- **Entities** (`GotchaUser`, `Player`, `Game`, …) — they live in `Gotcha.Core` because they extend `IdentityUser` and need EF Core attributes. MAUI never touches them directly.
- **DTOs / ViewModels / Request models** — those belong to the consuming project (API DTOs in `Gotcha.API/Dtos/`, MAUI models in `Gotcha.Maui/Models/`). DTOs are intentionally per-project even when identical, so each side can evolve independently. See `CLAUDE_API.md`.
- **Services** (DB access, HTTP clients, email, payments) — Core / API-specific / MAUI-specific.
- **Any code that pulls in `Microsoft.EntityFrameworkCore`, `Microsoft.AspNetCore.*`, or `Microsoft.Maui.*`.** Adding those would force all referencing projects to pull in the same dependency tree and defeat the point of a minimal shared lib.

## Tech Stack

- .NET 10.0, class library
- `<ImplicitUsings>enable</ImplicitUsings>`
- `<Nullable>enable</Nullable>`
- **Zero NuGet packages**, **zero project references**. Keep it that way.

## Current Contents

| Folder         | File                     | Purpose                                                                                         |
| :------------: | :----------------------: | :---------------------------------------------------------------------------------------------: |
|    `Enums/`    |       `Plan.cs`          | Subscription tiers (`Standard`, `Premium`, `Deluxe`) used by `VipSettings`, store view, MAUI VM |
|  `Constants/`  |   `DevConstants.cs`      | Fixed seed-user IDs (`TestUserId`, `TestUser2Id`, `TestUser3Id`) referenced by Seeder + MAUI    |
|   `Helpers/`   |    `AgeHelper.cs`        | `IsUnder16(DateTime)` — shared age check used by API signup validation AND MAUI signup form     |

## Conventions

- **Namespaces mirror folder structure**: `Gotcha.Shared.Enums`, `Gotcha.Shared.Constants`, `Gotcha.Shared.Helpers`.
- **Block-scoped namespaces** (`namespace X { ... }`) to match the style used by `Gotcha.Core/Enums/*.cs`.
- **No side effects at type init** beyond constant parsing (`Guid.Parse`, enum declarations).
- **`public static class`** for helpers and constants. No instances, no DI.
- **Add XML doc summaries** on public helper methods if their behavior isn't obvious from the name.

## When adding something new here

Ask yourself:
1. Do at least **two different projects** need this exact type? If no → put it in the project that uses it.
2. Is it a **pure value** (enum / const / pure function) with no external dependencies? If it needs EF Core, ASP.NET, or MAUI → it doesn't belong here.
3. Does it **cross the HTTP boundary** (API ↔ MAUI)? If yes and it's not already an API DTO, Shared is the right home.

If all three are yes → add it here. Otherwise, pick the most specific project that needs it.

## No separate guide for subfolders

This project is small enough that everything fits in this single file. If it grows beyond a few dozen files, consider splitting by folder.
