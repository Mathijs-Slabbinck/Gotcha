# Mobile (Gotcha.Maui) — To-Do List

## Authentication

- [x] **Sign In** — Wire up `ExecuteSignInCommand` in `SignInViewModel` to call the API. On success, switch AppShell to the Authenticated User TabBar (`//UserHome`).
- [x] **Sign Up** — Wire up `ExecuteSignUpCommand` in `SignUpViewModel` to call the API. On success, show a "check your email" message.
- [x] **TabBar switching** — After login, swap from the Unauthenticated TabBar to the Authenticated User or Player TabBar. After logout, swap back. This needs to be handled in `AppShell.xaml.cs`.
- [x] **Remember Me** — Persist the session token/userId so the user stays logged in across app restarts. Decide on storage strategy (SecureStorage).

---

## Player ID Wiring

- [x] **Game tapped in GamesViewModel** — `ExecuteGameTappedCommand` now sets `SessionService.CurrentPlayerId = game.PlayerId` and navigates to `Routes.PlayerHome`.
- [x] **All Player ViewModels use `Guid.Empty`** — `PlayerHomeViewModel`, `ConfirmKillViewModel`, `PlayerAdminViewModel`, and `PlayerSettingsViewModel` now inject `SessionService` and read `_sessionService.CurrentPlayerId` instead of passing `Guid.Empty`.

---

## Confirm Kill Page

- [x] **ConfirmKillCommand** — Calls `IPlayerService.ConfirmKillAsync(_sessionService.CurrentPlayerId)`. On success: `DisplayAlertAsync` with "Kill confirmed!"; on failure: `ErrorMessage`.
- [x] **ConfirmDeathCommand** — Calls `IPlayerService.ConfirmDeathAsync(...)` with matching success/error feedback.
- [x] **ConfirmHunterKillCommand** — Calls `IPlayerService.ConfirmHunterKillAsync(...)` with matching success/error feedback.
- [x] **API endpoints** — `POST api/players/{id}/confirmkill`, `POST api/players/{id}/confirmdeath`, and `POST api/players/{id}/confirmhunterkill` added to `PlayersController`. Also added `POST api/players/{id}/rejectkill` for kill attempt failures.

---

## Player Admin Page

- [ ] **PlayerActionCommand** — Replace the placeholder alert with real logic (kick → `DELETE api/players/{id}`, toggle admin/spectator → `PATCH api/players/{id}` with `IsAdmin`/`IsSpectator`). API endpoints are ready.
- [ ] **CancelKillCommand** — Call `POST api/kills/{killId}/reject` (with `AdminPlayerId` in body) to cancel a pending kill.
- [ ] **SaveSettingsCommand** — Call `PATCH api/games/{gameId}/settings` (with `AdminPlayerId` + all settings fields in body).
- [ ] **StartGameCommand** — Call `POST api/games/{gameId}/start` (with `AdminPlayerId` in body).
- [ ] **EndGameCommand** — Call `POST api/games/{gameId}/end` (with `AdminPlayerId` in body).
- [x] **API endpoints** — `POST api/games/{id}/start`, `POST api/games/{id}/end`, `PATCH api/games/{id}/settings`, `POST api/kills/{id}/validate`, `POST api/kills/{id}/reject`, `DELETE api/players/{id}` all added and building clean.

---

## Privacy Dashboard Page

- [ ] **Build the page** — `PrivacyDashboard.xaml` is a placeholder ("Welcome to .NET MAUI!"). Build it out with real content (delete account, download data, etc.).
- [ ] **Create PrivacyDashboardViewModel** — No ViewModel exists yet.
- [ ] **Register the route** — Add `Routing.RegisterRoute` for `PrivacyDashboard` in `MauiProgram.cs` and link to it from the Settings page.

---

## General / Cleanup

- [x] **Move Unauthenticated TabBar to startup position** — Unauthenticated TabBar is `Items[0]` in `AppShell.xaml`. On startup, `AppShell.CheckRememberedSessionAsync` only switches to the User TabBar if SecureStorage holds a userId — otherwise the app lands on Sign In.
- [ ] **Remove `Guid.Empty` workarounds** — Once real auth + player selection is working, remove all `DevConstants.TestUserId` / `Guid.Empty` fallbacks from ViewModels and services.
