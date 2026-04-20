# Gotcha.Maui Conventions

## Conventions

Before writing code in this subproject, read these convention files from `.claude/conventions/`:
- `CLAUDE_CS.md` — C# code style, naming, OOP, LINQ
- `CLAUDE_DOTNET.md` — Shared .NET: DI, async, ResultModel, HttpClient
- `CLAUDE_MAUI.md` — .NET MAUI: MVVM, XAML, Shell, ObservableCollection
- `CLAUDE_GIT.md` — Commit conventions, Git Flow, branching

## Project Overview

.NET MAUI mobile app targeting Android, iOS, macOS Catalyst, and Windows. Uses MVVM with CommunityToolkit.

## Tech Stack

- .NET 10.0 with .NET MAUI
- CommunityToolkit.Mvvm (`ObservableObject`, `RelayCommand`)
- CommunityToolkit.Maui (converters, behaviors)
- FluentValidation for input validation
- Microsoft.Extensions.Http (`IHttpClientFactory` for API calls)
- Shell navigation (`Shell.Current.GoToAsync`)

## Page Structure

Pages mirror the Web areas:

|          Folder          |                    Pages                     |
| :----------------------: | :------------------------------------------: |
| `Pages/Unauthenticated/` | SignIn, SignUp, Contact, Info, ResetPassword  |
| `Pages/Authenticated/User/` | Home, Games, NewGame, Settings, Store     |
| `Pages/Authenticated/Player/` | Home, ConfirmKill, Settings, Admin      |

## API Integration

- MAUI app connects to Gotcha.API at `http://localhost:5208` (Android emulator: `http://10.0.2.2:5208`)
- `IHttpClientFactory` configured as named client `"GotchaApi"` in `MauiProgram.cs`
- 5 service interfaces with two implementations each:
  - `Services/Api/` — real API implementations (currently active)
  - `Services/Mock/` — mock implementations for offline dev
- Swap between them in `MauiProgram.cs` DI registrations
- `Gotcha.Shared/Constants/DevConstants.cs` has `TestUserId` (fixed GUID matching Seeder) — used until auth is wired up. `Gotcha.Maui/Constants/DevConstants.cs` keeps only the `UseMockServices` toggle.

### Service → API Endpoint Mapping

|           MAUI Method            |                  API Endpoint                   |
| :------------------------------: | :---------------------------------------------: |
|  `IUserService.GetProfileAsync`  |    `GET api/gotchausers/{userId}/profile`        |
| `IUserService.UpdateProfileAsync` |      `PUT api/gotchausers/{userId}`             |
| `IGameService.GetPendingGamesAsync` | `GET api/gotchausers/{userId}/games?status=pending` |
| `IGameService.GetActiveGamesAsync` | `GET api/gotchausers/{userId}/games?status=active` |
|  `IGameService.GetEndedGamesAsync` | `GET api/gotchausers/{userId}/games?status=ended` |
|  `IGameService.CreateGameAsync`  |            `POST api/games`                     |
| `IPlayerService.GetPlayerHomeDataAsync` |    `GET api/players/{playerId}/home`       |
| `IPlayerService.GetConfirmKillDataAsync` | `GET api/players/{playerId}/confirmkill` |
|  `IPlayerService.GetAdminDataAsync` |     `GET api/players/{playerId}/admin`       |
| `IPlayerService.GetPlayerUsernameAsync` |     `GET api/players/{playerId}`          |
| `IPlayerService.UpdatePlayerUsernameAsync` | `PATCH api/players/{playerId}`         |
| `IStoreService.GetStoreStateAsync` |     `GET api/vipsettings/{userId}`            |
|  `IStoreService.BuyFeatureAsync` |       `PATCH api/vipsettings/{userId}`          |
| `IContactService.SubmitAsync`    |            `POST api/logs`                      |

## DI & Routing

- All Pages and ViewModels are registered as **Transient** in `MauiProgram.cs` (never Singleton — state must not persist across navigations)
- Push routes (e.g., `ResetPassword`) are registered in `MauiProgram.cs` via `Routing.RegisterRoute` (before `builder` creation) — never in `AppShell.xaml.cs`

## Navigation

Shell TabBar navigation with three TabBars (authenticated user, authenticated player, and unauthenticated).

**Authenticated User TabBar** (shown first in AppShell.xaml for dev/testing — move below unauthenticated TabBar when auth is wired up):

|    Tab    |      Route      |      Page       |        Icon         |
| :-------: | :-------------: | :-------------: | :-----------------: |
|   Home    | `//UserHome`    | `Home.xaml`     | `tab_home.svg`      |
|   Games   | `//UserGames`   | `Games.xaml`    | `tab_games.svg`     |
| Settings  | `//UserSettings`| `Settings.xaml` | `tab_settings.svg`  |
|   Store   | `//UserStore`   | `Store.xaml`    | `tab_store.svg`     |

**Authenticated Player TabBar** (shown second in AppShell.xaml for dev/testing):

|      Tab      |        Route         |        Page         |           Icon            |
| :-----------: | :------------------: | :-----------------: | :-----------------------: |
|     Home      | `//PlayerHome`       | `Home.xaml`         | `tab_player_home.svg`     |
| Confirm Kill  | `//PlayerConfirmKill`| `ConfirmKill.xaml`  | `tab_confirm_kill.svg`    |
|   Settings    | `//PlayerSettings`   | `Settings.xaml`     | `tab_player_settings.svg` |
|    Admin      | `//PlayerAdmin`      | `Admin.xaml`        | `tab_admin.svg`           |

**Unauthenticated TabBar:**

|   Tab    |    Route    |     Page      |       Icon        |
| :------: | :---------: | :-----------: | :---------------: |
| Sign In  | `//SignIn`  | `SignIn.xaml`  | `tab_signin.svg`  |
| Sign Up  | `//SignUp`  | `SignUp.xaml`  | `tab_signup.svg`  |
|   Info   | `//Info`    | `Info.xaml`    | `tab_info.svg`    |
| Contact  | `//Contact` | `Contact.xaml` | `tab_contact.svg` |

- Tab switching uses absolute routes (`//SignIn`, `//UserHome`, etc.)
- Push routes: `ResetPassword` (pushed on top of Sign In tab), `NewGame` (pushed on top of Games tab)
- Shell navbar hidden globally (`Shell.NavBarIsVisible="False"` on Shell element), except NewGame page (shows back button)
- Tab bar styled via `Styles.xaml`: OffBlack background, Primary cyan selected, Gray200 unselected

## MVVM Rules

- ViewModels must **never** access the View layer directly (no `Application.Current.Windows`, no `Page.DisplayAlertAsync`)
- Use `Shell.Current.DisplayAlertAsync` for alerts from ViewModels
- Commands must point to separate named Execute methods — never use inline async lambdas. Put `try/catch` inside the `async void` Execute method to prevent silent crashes
- Add an `IsBusy` property to ViewModels that make network calls; bind `IsEnabled` on buttons to prevent double-submits
- Every page with bindings **must** declare `x:DataType` on the `ContentPage` element for compiled bindings

## ViewModel Patterns

### LoadData in OnAppearing, not in constructors

`LoadData()` must be `public async void` on the ViewModel. The page code-behind calls it from `OnAppearing`:

```csharp
// ViewModel — public, no call in constructor
public async void LoadData() { ... }

// Page code-behind
protected override void OnAppearing()
{
    base.OnAppearing();
    _viewModel.LoadData();
}
```

### IsBusy + ErrorMessage in every LoadData

Every ViewModel with a `LoadData()` must have `IsBusy` and `ErrorMessage` properties. Wrap the body:

```csharp
public async void LoadData()
{
    try
    {
        IsBusy = true;
        ErrorMessage = string.Empty;
        // ... load data ...
    }
    catch
    {
        ErrorMessage = "Something went wrong loading ...";
    }

    IsBusy = false;
}
```

### Clear collections before reload

If `LoadData()` appends to `ObservableCollection` via `.Add()`, call `.Clear()` before the foreach to prevent duplicates on re-navigation:

```csharp
Players.Clear();
foreach (var player in data.Players)
{
    Players.Add(player);
}
```

### No raw exception messages in UI

Never expose `ex.Message` to users — use fixed, user-friendly strings:

```csharp
// Bad
catch (Exception ex) { ErrorMessage = ex.Message; }

// Good
catch { ErrorMessage = "Something went wrong. Please try again."; }
```

### Flat try/catch — avoid nested try/finally

Don't nest `try/finally` inside `try/catch`. Keep one level with `IsBusy = false` after the try/catch block:

```csharp
try
{
    IsBusy = true;
    // ... work ...
}
catch
{
    ErrorMessage = "Something went wrong. Please try again.";
}

IsBusy = false;
```

### Route constants

Use `Routes.SignIn`, `Routes.UserStore`, etc. instead of magic strings like `"//SignIn"`.

## Converters

- CommunityToolkit.Maui converters are declared in `Resources/Styles/Converters.xaml` (merged in `App.xaml`)
- Custom converters go in the `Converters/` folder as classes, then get added to `Converters.xaml` with an `x:Key`
- Never register converters in `App.xaml.cs` code-behind

## App Configuration

- App icon: custom Gotcha crosshair SVG (OffBlack `#252729` background, cyan `#5AD6DE` foreground)
- Splash screen: same crosshair design on OffBlack background
- XAML source generation enabled (`MauiXamlInflator = SourceGen`)

## Fonts

Fonts downloaded from Google Fonts into `fonts/` (solution root) and copied to `Resources/Fonts/`. Registered in `MauiProgram.cs`:

|          File            |     Alias      |
| :----------------------: | :------------: |
| Nosifer-Regular.ttf      |    Nosifer     |
| Bungee-Regular.ttf       |    Bungee      |
| Roboto-Regular.ttf       |    Roboto      |
| Roboto-Bold.ttf          |   RobotoBold   |
| RobotoSlab-Regular.ttf   |   RobotoSlab   |
| RobotoSlab-Bold.ttf      | RobotoSlabBold |
| OpenSans-Regular.ttf     | OpenSansRegular |
| OpenSans-Semibold.ttf    | OpenSansSemibold |

Use `FontFamily="Alias"` in XAML (e.g., `FontFamily="RobotoSlab"`, not `Roboto_Slab`). The `.csproj` wildcard picks up all font files automatically.

## XAML Style Rules

- Never hardcode colors — use `{StaticResource ColorName}` for all colors (including `White`, `Danger`, etc.)
- Named colors are defined in `Resources/Styles/Colors.xaml` (includes `Danger` for error text)
- Add `AutomationId` to all interactive controls (entries, buttons, checkboxes) for UI testing
- Add `SemanticProperties.Description` to `ImageButton` and icon-only controls for accessibility
- Add `SemanticProperties.Hint` to tappable labels to indicate they are interactive
- If a `Span` is visually styled as a link (underline), either add a `TapGestureRecognizer` or remove the underline

## Reference Projects

- **Coding style** (C# patterns, MVVM structure, command definitions, code-behind): refer to `C:\MobileDev\oef\solutions\st-oe-rock-paper-scissors-Mathijs-Slabbinck` — use this as the authoritative example for how C# code should be structured (expression-bodied command properties, separate Execute methods, constructor injection pattern, etc.)
- **Design / styling** (XAML layout, colors, card design, visual structure): refer to the corresponding page in the Gotcha.Web project (`Gotcha.Web/Views/`). You can use Playwright to view the web page if needed, but it's not required — reading the Razor views and CSS is usually enough.

## Key Paths

- Pages: `Pages/Unauthenticated/` and `Pages/Authenticated/` (User/ + Player/)
- ViewModels: `ViewModels/` (SignInViewModel, SignUpViewModel, InfoViewModel, ContactViewModel, HomeViewModel, GamesViewModel, NewGameViewModel, SettingsViewModel, StoreViewModel, PlayerHomeViewModel, ConfirmKillViewModel, PlayerSettingsViewModel, PlayerAdminViewModel); `BaseViewModels/PageBaseViewModel.cs` holds the common `IsBusy` + `ErrorMessage` pair — all VMs with network calls extend it
- Services: `Services/` (interfaces: IUserService, IGameService, IPlayerService, IStoreService, IContactService)
- API Services: `Services/Api/` (ApiUserService, ApiGameService, ApiPlayerService, ApiStoreService, ApiContactService)
- Mock Services: `Services/Mock/` (MockUserService, MockGameService, MockPlayerService, MockStoreService, MockContactService)
- Models: `Models/` organised by role — `PageData/` (AdminData, ConfirmKillData, PlayerHomeData, StoreState, UserProfile), `Items/` (GameItem, KillItem, PlayerItem, AdminPlayerItem, AdminKillItem), `Forms/` (SignUpData), `Payloads/` (PlayerActionCommand, UpdateGameSettingsCommand — outgoing API request bodies)
- Enums: `Enums/` (AdminPlayerCommandActions — picks Kick/ToggleAdmin/ToggleSpectator for the player-admin action sheet)
- Shared Enums: use `Gotcha.Shared.Enums` (e.g., `Plan`) — MAUI references `Gotcha.Shared` for cross-boundary types
- Routes: `Routes.cs` (all Shell route constants — use `Routes.SignIn`, not `"//SignIn"`)
- Constants: `Constants/DevConstants.cs`
- Converters: `Converters/` (custom classes) + `Resources/Styles/Converters.xaml` (declarations)
- Extensions: `Extensions/` (HttpContentExtensions — `ReadJsonStringAsync` unwraps ASP.NET's JSON-encoded `BadRequest(string)` bodies)
- Styles: `Resources/Styles/Colors.xaml`, `Resources/Styles/Styles.xaml`, and `Resources/Styles/Converters.xaml`
- Images: `Resources/Images/`
- Fonts: `Resources/Fonts/`
