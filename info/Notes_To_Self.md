# Gotcha (Notes To Self:)

## info:
**Notes about things I learned, unconventional approaches (different from what we saw in class), and decisions I made that are new to me.**
**Used to keep track of why I made certain decisions and so I always have a full grasp of how everything works. Handy for presentations or when I need to explain something on the spot.**



### Note 1:
#### Use of ::deep in CSS

`.cshtml.css` files use attribute scoping (e.g. `[b-abc123]`) to isolate styles per component. But elements inside nested Razor blocks (`@if`, `@switch`, `@foreach`) sometimes don't get the scope attribute. When that happens, your styles won't apply to those elements. Use `::deep .className` to target elements that are missing the scope attribute. This tells the browser to match the selector even inside unscoped child elements.



### Note 2:
#### coverlet.collector, xunit & xunit.runner.visualstudio (packages) purpose

- **xunit** — The test framework itself. Gives you `[Fact]`, `[Theory]`, `Assert.Equal()`, etc. Without it you can't write or define tests.
- **xunit.runner.visualstudio** — The bridge between xUnit and Visual Studio's Test Explorer. Without it, your tests exist but VS can't discover or run them.
- **coverlet.collector** — Code coverage tool. Tracks which lines of your code were hit by tests and which weren't. Optional (tests run fine without it), but useful to spot missing coverage. Came pre-installed with the xUnit project template.



### Note 3:
#### Using IdentityUser\<Guid\> instead of the default IdentityUser (string Id)

By default, ASP.NET Identity uses `IdentityUser` which has a `string` Id (a GUID stored as text). The course slides follow this default.

In this project, all entities already use `Guid` for their Ids. To avoid mixing `string` and `Guid` keys (which would mean converting between the two everywhere), `GotchaUser` inherits from `IdentityUser<Guid>` instead of plain `IdentityUser`.

**What this changes:**
- `GotchaUser : IdentityUser<Guid>` — the base Id becomes `Guid` instead of `string`
- `GotchaDbContext : IdentityDbContext<GotchaUser, IdentityRole<Guid>, Guid>` — tells Identity the key type is `Guid`
- `AddIdentity<GotchaUser, IdentityRole<Guid>>` in Program.cs — registers your custom user and role types

**Why this is better for this project:**
- All existing foreign keys (`Player.UserId`, `VipSettings` shadow FK, etc.) stay as `Guid` with no conversion needed
- No mismatch between Identity's Id type and the rest of the domain model
- Cleaner and more consistent overall

**What to know if asked:**
- The course teaches the default `IdentityUser` (string key). This is a valid alternative that achieves the same result with a typed key.
- `IdentityUser<TKey>` is the generic base class. `IdentityUser` is just shorthand for `IdentityUser<string>`.
- The Identity tables (AspNetUsers, AspNetRoles, etc.) will use `uniqueidentifier` columns in SQL Server instead of `nvarchar`.



### Note 4:
#### The `required` keyword on entity properties

C# 11 introduced `required`, which forces a property to be set in object initializers. It turns a runtime null reference into a compile-time error, which is great for safety.

**Why we use `required` on `UserName` and `Email` in GotchaUser:**
- These are inherited from `IdentityUser<Guid>`, where they're declared as `string?` (nullable)
- Identity makes them nullable because some auth flows don't need a username (e.g., phone-only login)
- In our app, both are always required. We override them with `required` so the compiler forces them to be set when creating a `new GotchaUser { ... }`
- Without `required`, you could forget to set `UserName` and the compiler wouldn't warn you (since the base default is `null`)

**Why we don't use `required` on other entity properties (FirstName, LastName, etc.):**
- EF Core creates entity instances internally using reflection (parameterless constructor), not object initializers
- `required` only works with object initializers. EF Core doesn't go through that path when loading data from the database
- If you add `required` to regular entity properties, EF Core can't materialize objects from queries, which breaks database reads
- The CS8618 warnings on non-nullable properties without defaults are the accepted tradeoff for EF Core POCOs
- The database layer (`.IsRequired()` in `OnModelCreating`) handles the actual null enforcement

**Why not use `= string.Empty` as a default?**
- It silences the warnings, but assigning an empty string to a field that should always have a real value feels misleading
- **__Personal preference__**: if a field is required, it shouldn't pretend to have a value when it doesn't yet



### Note 5:
#### Uri.TryCreate, UriKind, and uri.Scheme

`Uri` is a built-in .NET class for parsing and inspecting URLs. Used in `IsAllowedImageUrl` to validate profile image URLs.

**Uri.TryCreate(url, UriKind.Absolute, out var uri)**
- Tries to parse a string into a `Uri` object. Returns `true` if it succeeds, `false` if the string isn't a valid URL.
- `UriKind.Absolute` means the URL must be complete (like `https://example.com/photo.jpg`). Relative paths like `/images/photo.jpg` would fail.
- `out var uri` stores the parsed result in a variable called `uri`, so we can inspect its parts afterwards (scheme, host, path, etc.).
- It's a safe alternative to `new Uri(url)` which would throw an exception on invalid input.

**uri.Scheme**
- The scheme is the protocol part of the URL — the bit before `://`.
- Examples: `"https://example.com"` → scheme is `"https"`, `"ftp://files.com"` → scheme is `"ftp"`
- `Uri.UriSchemeHttp` is just the string `"http"`, `Uri.UriSchemeHttps` is just `"https"` (built-in constants so you don't hardcode strings)
- We only allow `http` and `https` to block dangerous schemes like `javascript:` (XSS), `file:` (local file access), `data:` (embedded content), and `ftp:`



### Note 6:
#### MAUI Splash Screen

The splash screen is the image shown briefly when the app first launches, before the first page loads. It's configured in the `.csproj` file:

```xml
<MauiSplashScreen Include="Resources\Splash\splash.svg" Color="#252729" BaseSize="128,128" />
```

- **`Include`** — points to the SVG (or PNG) image file
- **`Color`** — the background color that fills the screen behind the image
- **`BaseSize`** — the size the image is rendered at (MAUI scales it for different screen densities)

The splash SVG is separate from the app icon. The app icon uses two files (`appicon.svg` for the background + `appiconfg.svg` for the foreground), while the splash screen is a single image on a solid color background.



### Note 7:
#### IdentityDbContext gives you a `Users` DbSet for free

When your `GotchaDbContext` extends `IdentityDbContext<GotchaUser, IdentityRole<Guid>, Guid>`, the base class automatically registers several DbSets — including `Users` (a `DbSet<GotchaUser>`), `Roles`, `UserRoles`, `UserClaims`, `UserLogins`, `UserTokens`, and `RoleClaims`. All the `AspNet*` tables get created without you declaring them. That's the whole point of extending `IdentityDbContext`.

**What this means for `GotchaUsers`:**
- The explicit `public DbSet<GotchaUser> GotchaUsers { get; set; }` in the DbContext is a duplicate of the inherited `Users` property
- Both point to the exact same database table (`AspNetUsers`) — there's only 1 table, not 2
- You could remove `GotchaUsers` entirely and just use `_context.Users` everywhere

**What if you renamed it to `Users`?**
- It would still work, but it **shadows** (hides) the base class property
- The compiler gives a `CS0108` warning suggesting you add the `new` keyword
- Using `override` wouldn't work because the base `Users` property isn't `virtual`
- Either way, it's unnecessary — just remove it and use the inherited `Users`



### Note 8:
#### Privacy Policy Page — why we need one and what it does

A privacy policy is a legal document that tells users what personal data you collect, why, how you use it, and what rights they have. It's not a technical feature — it's a transparency/legal requirement.

**Why we need it:**
- We collect personal data: names, emails, birth dates, gender, guardian emails, profile images, IP addresses (via Attacker logging)
- Under GDPR (Belgium), any site that processes personal data must have a privacy policy. No exceptions.
- It must be easily accessible from every page without logging in

**What it covers:**
- Who you are (the data controller)
- What data you collect and why (each field: name, email, birthday, gender, profile picture, guardian email)
- How long you keep it (retention)
- Who you share it with (nobody — we don't share with third parties)
- User rights under GDPR (access, correction, deletion, portability, objection)
- Cookies and session info (we use one essential session cookie, no tracking)
- Children's data (guardian consent for under 16)
- Security logging (IP, user agent for suspicious activity)
- Data deletion policy (deleted means actually deleted)

**How it relates to the signup modals:**
- The signup page already has info icons that explain each field inline (great UX at the point of data collection)
- The privacy policy serves a different purpose: it's the single, complete, legally-referenceable document that covers everything in one place
- The modals explain things in the moment; the privacy policy is the full official overview anyone can read anytime
- The privacy policy also covers things that don't fit in signup modals (cookies, security logging, user rights, contact info)

**Where it lives:**
- `HomeController.Privacy()` action, `Views/Home/Privacy.cshtml` view
- Accessible at `/Home/Privacy` (public, no login required)
- Linked via a subtle footer on every page (all 3 layouts)



### Note 9:
#### Footer with Privacy Link — why it's on every page

GDPR requires the privacy policy to be easily accessible from every page — not just available at a URL. The industry standard is a footer link.

**Implementation:**
- Added a `<footer>` to all 3 layout files (public, User area, Player area)
- Shows: `© Mathijs Slabbinck · Privacy`
- `position: fixed` at the bottom, small text, subtle colors — doesn't add height or cause overflow
- `pointer-events: none` on the footer so it doesn't block page content; `pointer-events: auto` on the link so it's still clickable
- Area layouts use `asp-area=""` to route the link to `/Home/Privacy` instead of `/User/Home/Privacy`



### Note 10:
#### robots.txt — controlling what crawlers can access

`robots.txt` is a file at the root of your site that tells web crawlers (search engine bots, AI scrapers) which pages they're allowed to visit. It's a standard protocol that well-behaved bots respect.

**What ours does:**
- Blocks all crawlers from `/User/`, `/Player/`, `/Accounts/`, `/Gotcha/` (honeypot)
- AI crawlers (GPTBot, ClaudeBot, CCBot, etc.) get extra restrictions — also blocked from `/SignUp/` and `/ResetPassword/`
- Public pages (`/`, `/Info/`, `/Contact/`, `/Home/Privacy`) stay open so the app is findable on Google
- The goal: when someone searches "Gotcha" they find the app. When someone searches for a user's name, nothing Gotcha-related shows up.

**Important to know:**
- `robots.txt` is a request, not enforcement. Well-behaved bots follow it, malicious ones ignore it.
- That's why we also have other layers of protection (see Note 11 and Note 12)

**Where it lives:** `wwwroot/robots.txt` → served at `/robots.txt`



### Note 11:
#### Meta noindex tags — second layer of crawler protection

Even if a bot ignores `robots.txt` and visits a protected page, the `<meta name="robots" content="noindex, nofollow">` tag in the page's `<head>` tells it: "don't add this page to your search index, and don't follow any links on it."

**Where we added them:**
- User area layout (`Areas/User/Views/Shared/_Layout.cshtml`)
- Player area layout (`Areas/Player/Views/Shared/_Layout.cshtml`)
- NOT on public pages (those should be indexed)

**Three layers of protection:**
1. `robots.txt` — "don't visit these pages"
2. `<meta noindex>` — "if you do visit, don't index"
3. `[Authorize]` (once wired up) — blocks access entirely for unauthenticated users



### Note 12:
#### sitemap.xml — telling search engines what TO index

The opposite of robots.txt. Instead of saying "don't go here," a sitemap says "these are the pages I want you to find." It lists your public pages with priority rankings so search engines know what's important.

**Our sitemap lists:**
- `/` (login page) — priority 1.0
- `/Info` — priority 0.8
- `/SignUp` — priority 0.7
- `/Contact` — priority 0.5
- `/Home/Privacy` — priority 0.3

**Important:** The domain in the sitemap is a placeholder (`gotcha.example.com`). Replace it with the real domain when deploying.

**Where it lives:** `wwwroot/sitemap.xml` → served at `/sitemap.xml`
Referenced in `robots.txt` via a `Sitemap:` directive so crawlers find it automatically.



### Note 13:
#### manifest.json — PWA manifest

A web app manifest describes your app for browsers — name, icons, theme colors. It's what allows "Add to Home Screen" on mobile and controls how the app looks when launched from there.

**Our manifest:**
- Name: "Gotcha"
- Theme color: `#5AD6DE` (our primary blue)
- Background color: `#252729` (our dark background)
- Icons: references the android-icon PNGs in `/images/favicons/`

**The 404 fix:**
- The layouts referenced `/manifest.json` but the file was only inside `/images/favicons/manifest.json`
- Created a proper manifest at `wwwroot/manifest.json` (the root) so the path resolves correctly
- This fixes the console 404 errors we were seeing

### Note 14:
#### asp-validation-summary

Renders a `<div>` that collects and displays validation errors. Three modes:

- **`None`** — shows nothing (default)
- **`ModelOnly`** — only shows errors added with `string.Empty` as the key (model-level errors from the controller, like `ModelState.AddModelError(string.Empty, "Invalid email or password.")`)
- **`All`** — shows both field-level errors (from `[Required]`, `[EmailAddress]`, etc.) AND model-level errors

We use `All` on the SignUp form because Identity's `CreateAsync` returns errors (like "Password must have at least 12 characters") that get added with `string.Empty` — they're model-level, not tied to a specific field. Without `All`, those wouldn't show up.

`asp-validation-for` on individual fields shows only that field's errors. `asp-validation-summary` collects them all in one spot.

### Note 15:
#### Html.GetEnumSelectList\<T\>()

Generates `<option>` tags automatically from a C# enum. Instead of hardcoding options in the view:

```html
<!-- hardcoded (bad) -->
<select>
    <option>Male</option>
    <option>Female</option>
    <option>Other</option>
</select>

<!-- enum-driven (good) -->
<select asp-for="Gender" asp-items="Html.GetEnumSelectList<Genders>()">
    <option disabled selected value="">Select a Gender</option>
</select>
```

If you add or rename a value in the `Genders` enum, the dropdown updates automatically — no view changes needed. The placeholder option (`disabled selected value=""`) stays because it's defined separately from the generated items.

### Note 16:
#### Mapping Identity errors to specific fields

By default, Identity errors from `CreateAsync` get added with `string.Empty` as the key — meaning they're model-level and only show up in `asp-validation-summary`, not next to the field they belong to.

**The fix:** map error codes to ViewModel property names so they show next to the right field via `asp-validation-for`.

Identity error codes always contain the field name: `"PasswordTooShort"`, `"PasswordRequiresUpper"`, `"DuplicateEmail"`, `"InvalidUserName"`, etc. So we use `Contains` instead of a big switch/case:

```csharp
private static string GetFieldForIdentityError(string errorCode)
{
    if (errorCode.Contains("Password"))
        return "Password";

    if (errorCode.Contains("Email"))
        return "Email";

    if (errorCode.Contains("UserName"))
        return "UserName";

    return string.Empty;
}
```

**Why `Contains` over `StartsWith`:** some codes like `"InvalidUserName"` don't start with the field name.

**Why `string.Empty` fallback:** if an unexpected error code shows up, it falls back to model-level — shows in `asp-validation-summary="ModelOnly"` at the top. No error is silently lost.

**Result with `ModelOnly`:**
- Field errors → `asp-validation-for` shows them next to the right input
- Model-level errors → `asp-validation-summary="ModelOnly"` catches anything unexpected at the top
- No duplication (which `All` would cause — same error shown twice)



### Note 17:
#### MAUI App.xaml.cs — `CreateWindow` and `IActivationState`

**The class:**
`App` is the root of a MAUI app. It is `partial` because the other half is generated from `App.xaml` (which contains resource dictionaries, global styles, etc.). `InitializeComponent()` loads that XAML half.

**Why we inject AppShell instead of writing `new AppShell()`:**
`AppShell` now requires `SessionService` in its constructor. If you write `new AppShell()`, it crashes because that parameter is missing. By registering `AppShell` as a singleton in `MauiProgram.cs` and injecting it into `App`, the DI container builds the whole chain automatically: `App` → `AppShell` → `SessionService`. MAUI supports constructor injection in `App` because `UseMauiApp<App>()` registers `App` in the DI container.

**`CreateWindow(IActivationState? activationState)`:**
MAUI calls this method when it needs to create the OS-level window that holds all the UI:
- On **Android** → maps to an `Activity`
- On **iOS** → maps to a `UIWindow`
- On **Windows** → maps to a WinUI `Window`

You override it (instead of setting a property in the constructor) to control exactly what goes inside that window.

**`IActivationState? activationState`:**
This tells you *how* the app was opened:
- Normal launch → `null` or essentially empty
- Opened from a push notification → contains the notification payload
- Opened via a URL scheme / deep link → contains the URL

It is nullable (`?`) because not every launch has a specific context. In most apps you ignore it. If you wanted **deep linking** (e.g. "tap a notification, land on a specific game page"), you'd read `activationState` here and navigate accordingly.

**`return new Window(_appShell)`:**
Wraps the injected `AppShell` in an OS window and hands it to MAUI to display. `AppShell` is the root navigation container — it defines the tab bars and all Shell routes. Passing it to `Window` is what actually makes it appear on screen.



### Note 18:
#### `ResultModel` logging vs. the MAUI client — different layers

I was worried that using a simple tuple (`Task<(bool Success, string? ErrorMessage)>`) on the MAUI side for API calls would "defeat the point" of `ResultModel<T>` by losing the logging/warnings infrastructure. It doesn't — they solve different problems and live on different sides of the HTTP boundary.

**Where `ResultModel` actually works:**
`ResultModel<T>` lives in `Gotcha.Core` and is used inside `RepoService` / `GameService` methods. When any of those fail, they:
1. Capture the error as a typed `Error` entity (with `LogSubTypes` category),
2. Hand it to `LogRepoService` which **persists it to the DB**,
3. Return the `ResultModel` up the stack.

This all happens **inside the server** when an API controller calls into Core. The MAUI client doesn't trigger any of this directly — it triggers it *by calling the API*. Server-side logging is already handled no matter what shape the client uses.

**What crosses the wire:**
The API controller reduces the rich `ResultModel` down to a simple HTTP response:
```csharp
catch (GotchaException ex)
{
    return BadRequest(ex.Message);  // single string on the wire
}
```
The client never sees the `Warning` entities, `LogSubTypes`, or the structured error list — and doesn't need to.

**What the MAUI client actually needs:**
Just the success flag + the user-facing message. That's exactly what a `(bool Success, string? ErrorMessage)` tuple gives. A full `ResultModel` copy on the client would be duplicated machinery for no gain — MAUI doesn't write to a DB log table; it just shows messages.

**If you later want mobile telemetry** (crash reports, device-side logs), that's a separate concern handled by tools like App Insights or Sentry — not something the service-return-type should try to carry.

**Rule of thumb:**
- Server code that touches data → `ResultModel<T>`.
- Server code that serves HTTP → DTOs + status codes + error messages.
- Client code that consumes HTTP → tuple or lightweight result type carrying just what the UI shows.



### Note 19:
#### `async void LoadData()` + `private async Task LoadDataAsync()` — why the split in ConfirmKillViewModel

**Convention:** MAUI ViewModels expose `public async void LoadData()`. The page code-behind calls it from `OnAppearing()` without `await`:
```csharp
protected override void OnAppearing()
{
    base.OnAppearing();
    _viewModel.LoadData();
}
```
`async void` is acceptable here because `OnAppearing` doesn't need to wait — the screen renders immediately, data fills in when the call returns, `IsBusy` gates the UI.

**The problem that made me split it:**
After the `ConfirmKill` / `ConfirmDeath` / `ConfirmHunterKill` commands succeed, the page state is stale (the old target info is still shown). So I wanted to call `LoadData()` again after success, inside `RunConfirmAsync`:

```csharp
if (success)
{
    await Shell.Current.DisplayAlertAsync(...);
    LoadData();          // fire-and-forget
    // ↓ falls through
}
IsBusy = false;          // runs BEFORE LoadData's network call finishes!
```

`LoadData()` being `async void` means `RunConfirmAsync` cannot `await` it. The synchronous portion of `LoadData` runs (sets `IsBusy = true`), then on the first `await` inside (`_playerService.Get...`) control returns to `RunConfirmAsync`, which promptly sets `IsBusy = false`. Net result: during the network refresh, buttons are enabled and the user can tap another Confirm button → race.

**The fix (without breaking the convention):**
```csharp
public async void LoadData()         // convention-compatible entry point for OnAppearing
{
    await LoadDataAsync();
}

private async Task LoadDataAsync()   // real work, awaitable from RunConfirmAsync
{
    // IsBusy = true / fetch / assign properties / IsBusy = false
}
```

`RunConfirmAsync` now does `await LoadDataAsync();` — `IsBusy` stays `true` through the whole reload, buttons stay disabled, no race.

**Alternative considered:** make `LoadData` itself return `Task` (`public async Task LoadData()`). Cleaner — one method. But:
- Every page code-behind has to change from `_viewModel.LoadData();` to either `_ = _viewModel.LoadData();` or `await _viewModel.LoadData();` (requiring `async void OnAppearing`).
- Only `ConfirmKillViewModel` needs internal await. The other 11 VMs don't.
- Changing the convention for all 12 VMs to satisfy one use case = more churn than one small helper method.

**Takeaway:**
- Convention exists because it keeps OnAppearing simple across pages.
- When one VM needs internal await, keep the `async void` wrapper and extract a `private async Task` helper — honor both the convention and the correctness requirement.
