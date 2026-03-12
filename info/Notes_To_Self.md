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
