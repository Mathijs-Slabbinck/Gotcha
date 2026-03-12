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
- Shell navigation (`Shell.Current.GoToAsync`)

## Page Structure

Pages mirror the Web areas:

|          Folder          |                    Pages                     |
| :----------------------: | :------------------------------------------: |
| `Pages/Unauthenticated/` | SignIn, SignUp, Contact, Info, ResetPassword  |
| `Pages/Authenticated/User/` | Home, Games, Settings, Store, Shared      |
| `Pages/Authenticated/Player/` | Home, ConfirmKill, Settings, Admin      |

## DI & Routing

- All Pages and ViewModels are registered as **Transient** in `MauiProgram.cs` (never Singleton — state must not persist across navigations)
- Shell routes for non-tab pages (SignUp, ResetPassword, Contact, Info) are registered in `AppShell.xaml.cs` via `Routing.RegisterRoute`
- Navigate with relative routes (`Shell.Current.GoToAsync("SignUp")`) for registered routes

## MVVM Rules

- ViewModels must **never** access the View layer directly (no `Application.Current.Windows`, no `Page.DisplayAlertAsync`)
- Use `Shell.Current.DisplayAlertAsync` for alerts from ViewModels
- Commands must point to separate named Execute methods — never use inline async lambdas. Put `try/catch` inside the `async void` Execute method to prevent silent crashes
- Add an `IsBusy` property to ViewModels that make network calls; bind `IsEnabled` on buttons to prevent double-submits
- Every page with bindings **must** declare `x:DataType` on the `ContentPage` element for compiled bindings

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
- ViewModels: `ViewModels/` (SignInViewModel, SignUpViewModel, InfoViewModel, ContactViewModel)
- Converters: `Converters/` (custom classes) + `Resources/Styles/Converters.xaml` (declarations)
- Extensions: `Extensions/` (currently empty)
- Styles: `Resources/Styles/Colors.xaml`, `Resources/Styles/Styles.xaml`, and `Resources/Styles/Converters.xaml`
- Images: `Resources/Images/`
- Fonts: `Resources/Fonts/`
