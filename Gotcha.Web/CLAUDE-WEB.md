# Gotcha.Web Conventions

## Conventions

Before writing code in this subproject, read these convention files from `.claude/conventions/`:
- `CLAUDE_CS.md` — C# code style, naming, OOP, LINQ
- `CLAUDE_DOTNET.md` — Shared .NET: DI, async, ResultModel, HttpClient
- `CLAUDE_MVC_WEB-BACKEND.md` — ASP.NET MVC: controllers, Razor, EF Core, Identity
- `CLAUDE_HTML.md` — HTML5 structure, forms, accessibility
- `CLAUDE_CSS.md` — Box model, selectors, flexbox, grid, responsive
- `CLAUDE_JAVASCRIPT.md` — JS strict mode, DOM, events, fetch
- `CLAUDE_GIT.md` — Commit conventions, Git Flow, branching

## MVC Areas

The web app uses three routing contexts:

| Route prefix | Area | Purpose |
|---|---|---|
| `/` | (none) | Public pages: login, signup, info, contact, password reset |
| `/User/` | User | Authenticated user dashboard, games, settings, store |
| `/Player/` | Player | Player-specific game views: home, confirm kill, settings, admin |

Each area has its own `_Layout.cshtml`. The Player area uses partials for alive/dead navbars. The Gotcha honeypot page uses `_GotchaLayout.cshtml` (no hamburger menu, only "Take me back" link).

## Frontend Stack

- Razor views with Bootstrap 5, jQuery, jQuery Validation, Cropper.js (image crop)
- Google Fonts: Nosifer, Bungee, Roboto Slab, Roboto
- CSS variables defined in `wwwroot/css/Variables.css`
- Page-specific JS and CSS files live alongside the shared `Site.css`
- Responsive design with breakpoints from 500px to 1700px

## CSS Rules

- Prefer `vh` and `vw` units unless something else is clearly better
- When multiple elements show info-only modals, use a single shared modal and update its content via JS — not separate modals per element (see SignUp page for reference)
- No inline `style` attributes in HTML — always put styling in the correct CSS file

## CSS Isolation Notes

- `.cshtml.css` files use attribute scoping (`[b-abc123]`)
- Elements inside nested Razor blocks (`@if`, `@switch`) may not get the scope attribute
- Use `::deep .className` to target those elements
- `:not(:last-of-type)` only works when elements are direct siblings in the same parent
- Bootstrap `.row` adds negative margins — be careful wrapping cards in rows (affects `col-*` width)

## Key Paths

- CSS/JS: `wwwroot/css/` (Site.css, Layout.css, Variables.css) and `wwwroot/js/` (signUpPage.js, signUpValidation.js, shared/imageValidation.js, shared/site.js)
- Libs: `wwwroot/lib/` (bootstrap, jquery, jquery-validation, jquery-validation-unobtrusive, cropperjs)
- Player ViewModels: `Areas/Player/ViewModels/` (HomeViewModel + BaseViewModels/)
- Account Views: `Views/Accounts/` (EmailConfirmed, EmailConfirmationFailed, GuardianConsentConfirmed, GuardianConsentFailed)
- Privacy Dashboard: `Areas/User/Controllers/PrivacyDashboardController.cs` + `Areas/User/Views/PrivacyDashboard/`
