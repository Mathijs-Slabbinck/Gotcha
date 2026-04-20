# TODO Notes — Deferred / Needs Investigation

Things that came up during work but were too fuzzy or out-of-scope to fix on the spot. Each entry should have enough context to pick back up later.

---

## Shell tab-switch staleness in Player ViewModels

**Symptom (theoretical — needs on-device verification):**
After the Player ID wiring work, `PlayerHomeViewModel` / `ConfirmKillViewModel` / `PlayerAdminViewModel` / `PlayerSettingsViewModel` load data using `_sessionService.CurrentPlayerId` inside their `LoadData()` method, which fires from the page's `OnAppearing`.

The concern: if a user
1. opens PlayerHome for Game A (LoadData runs, `CurrentPlayerId` = PlayerA),
2. navigates back to `//UserGames`,
3. taps Game B (`SessionService.CurrentPlayerId` is now PlayerB),
4. lands back on PlayerHome via `//PlayerHome`,

will `OnAppearing` fire again on the cached PlayerHome instance, or is stale PlayerA data shown?

**What's uncertain:**
- MAUI Shell with `DataTemplate` in `<TabBar>` normally creates pages lazily and caches them.
- ViewModels are registered **Transient**, but that only matters when the framework *asks* for a new one — if Shell reuses the page instance, the same ViewModel instance is reused too.
- Absolute route navigation (`//PlayerHome`) *usually* does trigger `OnAppearing`, but tab-to-tab switches within the same TabBar might not.

**Why deferred:**
Can't determine the actual behavior without running the app on a device/emulator. The fix depends on which behavior we observe:
- If `OnAppearing` fires reliably → no-op, nothing to do.
- If it doesn't fire on tab switches → either (a) override `OnNavigatedTo` instead of `OnAppearing`, (b) register the ViewModels as Singleton + subscribe to a session-change event, or (c) expose a `PlayerIdChanged` event on `SessionService` and have ViewModels reload on it.

**Files involved:**
- `Gotcha.Maui/Pages/Authenticated/Player/Home.xaml.cs` (and the other 3 Player page code-behinds)
- `Gotcha.Maui/Services/SessionService.cs`
- `Gotcha.Maui/ViewModels/PlayerHomeViewModel.cs` (+ ConfirmKill, PlayerAdmin, PlayerSettings)

**When to tackle:**
Next time the app is run interactively — reproduce the Game A → Game B flow and see what actually happens. Fix at that point with full knowledge.

---

## PayPal sandbox credentials — rotate + move out of git

**Current state:**
- `DotNetEnv` package added to `Gotcha.Web`.
- `Gotcha.Web/Program.cs` loads `.env` via `DotNetEnv.Env.TraversePath().Load()` before `WebApplication.CreateBuilder(args)`.
- `.env` and `.env.*` are in `.gitignore`.
- `.env.example` committed at repo root documents which keys the app expects.
- Production secrets would come from a cloud secret manager (Azure Key Vault etc.) — layered config takes care of that when we deploy.

**What still needs to happen manually:**

1. **Rotate the leaked sandbox credentials.** The original `ClientId` (`AcJ…hG2`) and `Secret` (`EDD…Yhf`) were pasted into `Gotcha.Web/appsettings.Development.json` at some point and therefore showed up in an AI conversation log. Treat them as compromised:
   - Go to https://developer.paypal.com/dashboard/applications/sandbox
   - Generate a new credential pair (or delete & recreate the sandbox app).
   - Put the new values in `Gotcha/.env` (repo root, gitignored):
     ```
     PayPal__ClientId=<new value>
     PayPal__Secret=<new value>
     ```

2. **Remove the credentials from `Gotcha.Web/appsettings.Development.json`.** Keep `PayPal:BaseUrl` (not a secret). Target content:
   ```json
   {
       "Logging": { ... },
       "EmailSettings": { ... },
       "PayPal": {
           "BaseUrl": "https://api-m.sandbox.paypal.com"
       }
   }
   ```

3. **Optional cleanup:** the `<UserSecretsId>` line in `Gotcha.Web.csproj` was added earlier and is no longer used. Safe to leave (does nothing without a secrets store) or safe to delete.

**When deploying for real later:**
Register a cloud secret provider in `Program.cs` gated on `!builder.Environment.IsDevelopment()`. Example with Azure Key Vault:
```csharp
if (!builder.Environment.IsDevelopment())
{
    builder.Configuration.AddAzureKeyVault(
        new Uri("https://<your-vault>.vault.azure.net/"),
        new DefaultAzureCredential());
}
```
`IConfiguration` consumers (`IOptions<PayPalSettings>`, etc.) don't change — they just pull from a different source in prod.
