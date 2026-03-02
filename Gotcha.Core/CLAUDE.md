# Gotcha.Core Conventions

## Entity Conventions

- Entities are **POCOs** — no validation in property setters, no business logic
- Auto-properties with initializers, no backing fields unless needed
- `{ get; init; }` for IDs and timestamps (locked after creation)
- No constructors on entities — use object initializer syntax
- Computed properties (pure reads) can stay on the entity

```csharp
public class Entity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public string Name { get; set; } = "default";
    // Computed properties (pure reads) can stay on entity
}
```

## Database Conventions

- TimeSpan stored as ticks in DB (configured in `OnModelCreating`)
- `GotchaDbContext` extends `IdentityDbContext` (not plain `DbContext`)
- Cascade behavior: Game→Players (Cascade), Player→GotchaUser (Restrict), Kill→Killer/Victim (Restrict)
- Seeder uses `GotchaDbContext` (`SeedAsync(GotchaDbContext)`) not `ModelBuilder.HasData()`
- CS8618 warnings on navigation properties are normal for EF Core POCOs

## Service Patterns

```
Controller → Service (validates + business logic) → RepoService (CRUD) → DbContext → DB
```

- **Repository pattern**: `IRepositoryService<T>` with concrete implementations per entity in `Services/Repository/`. CRUD only, no business logic.
- **Service layer**: `GameService` handles orchestration (JoinPlayer, StartGame, HandleValidKill, etc.)
- **Result model**: `ResultModel<T>` / `BaseResultModel` for returning data with error/warning collections
- **Validation**: Split into 3 focused static services — `UserValidationService` (email, username, name, birthday), `ImageValidationService` (image URL, file extension, MIME type, file size, magic bytes, re-encoding), `SecurityValidationService` (IP validation). Reserved usernames = simple static HashSet, no config/DI.
- **Exception hierarchy**: Base `GotchaException` with specific subtypes (`GameStateException`, `GameRuleViolationException`, `ValidationException`, etc.) and per-entity `NotFoundException` classes
- **Logging**: Custom `Log` entity system with `Attacker` tracking (IP, user agent, path) and subtypes (Error, Warning, HackAttempt)

## DI Registrations (in Program.cs)

- All 7 RepoServices registered as Scoped
- GameService registered as Scoped
