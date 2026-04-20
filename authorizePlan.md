# Authorize Plan — Securing the API with JWT Bearer Auth

## Context

The `Gotcha.API` currently has `AuthController.cs` and `app.UseAuthentication()` in `Program.cs`, but no authentication scheme is wired up. `IAuthService.SignInAsync` returns just a `Guid? UserId` — meaning sign-in identifies the user but issues no credential. MAUI stores the `UserId` in `SessionService`, and every subsequent API call passes that ID in the route (e.g. `api/players/{playerId}/admin`). Any attacker with the API URL can impersonate any user by guessing a valid GUID.

Goal: real authentication so `[Authorize]` on controllers becomes enforceable, and each request proves who it's from.

**Why JWT over cookies**: MAUI doesn't maintain a cookie jar by default. Tokens travel cleanly in `Authorization` headers, survive app restarts via `SecureStorage`, and are the standard for mobile→API auth. `Gotcha.Web` keeps using cookie auth (Identity default) — the two auth schemes can coexist because they live in separate apps.

---

## Phase 1 — Server issues tokens

**Files touched:** `Gotcha.API/Program.cs`, `Gotcha.API/Controllers/AuthController.cs`, `Gotcha.API/Gotcha.API.csproj`, `Gotcha.API/appsettings.json`, `.env` / user-secrets.

1. Add package `Microsoft.AspNetCore.Authentication.JwtBearer` to `Gotcha.API`.
2. In `Program.cs`, register the scheme:
   ```csharp
   builder.Services
       .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = builder.Configuration["Jwt:Issuer"],
               ValidAudience = builder.Configuration["Jwt:Audience"],
               IssuerSigningKey = new SymmetricSecurityKey(
                   Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
           };
       });
   ```
3. Put the signing key in `.env` or user-secrets (`.env` is already in `.gitignore`). Use a cryptographically random key of at least 32 bytes. Add issuer and audience to `appsettings.json` (they're not secrets).
4. Update `AuthController.SignIn`:
   - Validate credentials via `UserManager.CheckPasswordAsync` on Identity.
   - Issue a signed JWT with claims: `sub` = UserId, `exp` = ~14 days.
   - Return `{ token, userId, expiresAt }`.
5. Update `IAuthService.SignInAsync` to return the token alongside `UserId`:
   ```csharp
   Task<(Guid? UserId, string? Token, DateTime? ExpiresAt, string? ErrorMessage)> SignInAsync(...);
   ```

**Verification:** Hit `POST api/auth/signin` in a REST client with valid credentials → receive JWT in the response body. Decode at jwt.io — claims should include `sub` and a future `exp`.

---

## Phase 2 — Client sends tokens

**Files touched:** `Gotcha.Maui/Services/Api/ApiAuthService.cs`, `Gotcha.Maui/ViewModels/SignInViewModel.cs`, `Gotcha.Maui/Services/SessionService.cs`, `Gotcha.Maui/MauiProgram.cs`, new `Gotcha.Maui/Services/Http/AuthHeaderHandler.cs`.

1. On successful sign-in, store the token in `SecureStorage` next to the existing `userId` key:
   ```csharp
   await SecureStorage.SetAsync("authToken", result.Token);
   await SecureStorage.SetAsync("userId", result.UserId.ToString());
   ```
2. Create a `DelegatingHandler` (`AuthHeaderHandler`) that reads the token and attaches it to every outgoing request:
   ```csharp
   public class AuthHeaderHandler : DelegatingHandler
   {
       protected override async Task<HttpResponseMessage> SendAsync(
           HttpRequestMessage request, CancellationToken cancellationToken)
       {
           string? token = await SecureStorage.GetAsync("authToken");
           if (!string.IsNullOrEmpty(token))
           {
               request.Headers.Authorization =
                   new AuthenticationHeaderValue("Bearer", token);
           }
           return await base.SendAsync(request, cancellationToken);
       }
   }
   ```
3. Register the handler on the `"GotchaApi"` `HttpClient` in `MauiProgram.cs`:
   ```csharp
   builder.Services.AddTransient<AuthHeaderHandler>();
   builder.Services
       .AddHttpClient("GotchaApi", client => { client.BaseAddress = ...; })
       .AddHttpMessageHandler<AuthHeaderHandler>();
   ```
4. On any 401 response, clear `SecureStorage` and navigate to `//SignIn`. Best place: a second `DelegatingHandler` that wraps the response, or a helper in `ApiPlayerService`/`ApiGameService` called after each failed request.

**Verification:** Sign in on the MAUI app, watch the API's dev logs, confirm the `Authorization` header is present on the next request.

---

## Phase 3 — Lock down endpoints

**Files touched:** all 6 API controllers.

1. Add `[Authorize]` at the top of each API controller:
   - `GamesController`
   - `KillsController`
   - `PlayersController`
   - `GotchaUsersController`
   - `VipSettingsController`
   - `RulesController`
2. Leave `AuthController` **unauthenticated** — sign-in and sign-up must be reachable anonymously. Either omit `[Authorize]` or use `[AllowAnonymous]` on the specific actions.
3. After this step, any MAUI request without a valid token returns 401. The `AuthHeaderHandler` 401-catch from Phase 2 handles that automatically.

**Verification:** Call any controller endpoint in a REST client **without** a token → 401. Call with a valid token → normal response.

---

## Phase 4 — Ownership checks (prevent horizontal privilege escalation)

**Files touched:** API controllers that take user/player IDs in the route.

The pre-session security review flagged resource enumeration (`"User not found."` messages reveal which GUIDs exist). `[Authorize]` alone doesn't fix that — a logged-in attacker can still probe any user's data by changing the GUID in the URL. Fix: inside each action, verify the token's `sub` claim matches the resource owner, OR require an admin role.

Two patterns to pick from:

### Pattern A — Keep `{userId}` in routes, verify it matches the token
```csharp
[HttpGet("{userId:guid}")]
public async Task<IActionResult> Get(Guid userId)
{
    Guid currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    if (currentUserId != userId && !User.IsInRole("Admin"))
    {
        return StatusCode(StatusCodes.Status403Forbidden, "You can only access your own profile.");
    }
    // ... existing logic
}
```

### Pattern B — Collapse to `/me` endpoints (cleaner, but touches MAUI too)
```csharp
[HttpGet("me")]
public async Task<IActionResult> GetMe()
{
    Guid currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    // ... use currentUserId, no ID in route
}
```

Pick one and apply consistently. Recommendation: **Pattern A** in the short term (minimal MAUI impact), migrate to Pattern B as a follow-up.

**Special cases:**
- `GamesController.Start/End/UpdateSettings` — already check admin via `game.AdminIds.Contains(adminPlayer.Id)`. Keep that check, but also verify `dto.AdminPlayerId` is the authenticated user (so a malicious client can't send another admin's ID).
- `PlayersController` actions on `{id}` — verify the player belongs to the authenticated user.
- `VipSettingsController.Patch` — must restrict to admins only, or the user can unlock premium features for free.

**Files touched:** 6 controllers, ~20 actions.

**Verification:** Sign in as user A, try to GET user B's profile → 403. Same for games A isn't in, kills A didn't make, etc.

---

## Phase 5 — Refresh tokens (optional, do later)

14-day access tokens are convenient but risky if stolen (an attacker with a stale token has 2 weeks of access). Standard fix:
- Shorten access-token lifetime to ~15 minutes.
- Issue a long-lived refresh token on sign-in, stored securely.
- Add `POST api/auth/refresh` that exchanges a refresh token for a new access token.
- MAUI calls refresh automatically when an access token is about to expire (tracked via `expiresAt`).

Skip for MVP; revisit before a production release.

---

## Cross-cutting concerns

- **Signing key management.** Dev uses `.env`. Production needs a secret manager (Azure Key Vault, AWS Secrets Manager). The key must be rotated on compromise — all existing tokens become invalid, every user must sign in again.
- **HTTPS only.** JWTs in headers are equivalent to passwords over the wire. Enforce HTTPS on the API (`app.UseHttpsRedirection()` already in `Program.cs` — confirm it's not bypassed).
- **Logging.** Don't log the token value itself. Log `userId` on successful requests, nothing on failures beyond the 401 status.
- **Identity already configured in Web**, not API. API currently has no `AddIdentity` call. Phase 1 needs `UserManager<IdentityUser>` which requires `AddIdentityCore<IdentityUser>().AddEntityFrameworkStores<GotchaDbContext>()` in API's `Program.cs`. The API doesn't need the cookie stack Web uses — just the password hasher and user store.

---

## Scope options for the next PR

| Scope                                              | Files | Outcome                                                               |
| :------------------------------------------------- | :---: | :-------------------------------------------------------------------- |
| **Minimal**: Phases 1–3                            |  ~10  | Tokens work, controllers require auth. Enumeration still possible via valid token. |
| **Full secure**: Phases 1–4                        |  ~20  | Production-ready for this scope. Enumeration blocked.                  |
| **Everything**: Phases 1–5                         |  ~25  | Includes refresh tokens; defer unless there's a release pressure.      |

Recommended: **Minimal first**, Phase 4 in a follow-up PR. Keeps diffs reviewable.

---

## Verification checklist (post-implementation)

- [ ] `POST api/auth/signin` with valid credentials returns a JWT.
- [ ] MAUI sign-in stores the token in `SecureStorage`.
- [ ] Subsequent MAUI requests include `Authorization: Bearer <token>`.
- [ ] Unauthenticated requests to any non-auth controller return 401.
- [ ] Expired tokens are rejected (test by setting a 1-minute `exp` locally).
- [ ] Invalid/tampered tokens are rejected.
- [ ] 401 in MAUI clears SecureStorage and redirects to Sign In.
- [ ] (Phase 4) Authenticated user A cannot read/write user B's data.
- [ ] `dotnet build` clean.
- [ ] `dotnet test Gotcha.Core.Tests` passes.

---

## Files the plan will touch (summary)

**API side:**
- `Gotcha.API/Program.cs`
- `Gotcha.API/Controllers/AuthController.cs`
- `Gotcha.API/Gotcha.API.csproj` (package)
- `Gotcha.API/appsettings.json` (Jwt:Issuer, Jwt:Audience)
- `.env` (Jwt:Key — NOT committed)
- All 6 non-auth controllers (add `[Authorize]`, Phase 3; ownership checks, Phase 4)

**MAUI side:**
- `Gotcha.Maui/Services/IAuthService.cs`
- `Gotcha.Maui/Services/Api/ApiAuthService.cs`
- `Gotcha.Maui/Services/Mock/MockAuthService.cs`
- `Gotcha.Maui/Services/Http/AuthHeaderHandler.cs` (new)
- `Gotcha.Maui/Services/SessionService.cs`
- `Gotcha.Maui/ViewModels/SignInViewModel.cs`
- `Gotcha.Maui/MauiProgram.cs`

**Deferred to Phase 5 (if ever):**
- `Gotcha.API/Controllers/AuthController.cs` (add `/refresh`)
- Refresh-token storage and handler in MAUI
