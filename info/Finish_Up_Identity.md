# Finish Up Identity — Session Notes

Reference file for guiding the user through implementing Identity & Authentication.
See `toDo.md` for the actual task checklist.

---

## Current State (as of 2026-03-19)

### What's Already Done

- **Identity configured in Program.cs**: `AddIdentity<GotchaUser, IdentityRole<Guid>>` with password policy (12 chars, digit, lower, upper, special, 4 unique), `AddEntityFrameworkStores`, `AddDefaultTokenProviders`
- **Middleware order is correct**: `UseRouting` > `UseSession` > `UseAuthentication` > `UseAuthorization`
- **GotchaUser extends IdentityUser<Guid>**: custom props (FirstName, LastName, BirthDate, Gender, ProfileImageSource, GuardianEmail, etc.)
- **GotchaDbContext extends IdentityDbContext<GotchaUser, IdentityRole<Guid>, Guid>**: Identity tables included
- **Identity migration exists** (applied 2026-03-15): AspNetUsers, AspNetRoles, etc. are in the DB
- **ViewModels exist**: `HomeViewModel` (login: UserNameOrEmail + Password), `SignUpViewModel` (full signup with custom validators)
- **Views exist**: Login form (Home/Index), SignUp form (SignUp/Index), ResetPassword form — all have client-side validation and UI, but forms are not wired to POST actions
- **Session configured**: in-memory cache, 30-min timeout, HttpOnly + Secure cookies
- **AccountsController**: has `Logout()` action (stub — just redirects, no SignOutAsync yet)

### What's NOT Done

- No `SignInManager` or `UserManager` injected in any controller
- No POST actions for login or signup
- No password hashing during signup (Seeder creates users without password hashes)
- No `[Authorize]` attributes anywhere
- No logout logic (just a redirect stub)
- No password reset logic
- No email service
- Forms in views have no `asp-action`/`asp-controller` attributes

---

## Key Files

| Purpose              | File                                                     |
| :------------------: | :------------------------------------------------------: |
| Identity config      | `Gotcha.Web/Program.cs`                                  |
| DbContext            | `Gotcha.Core/Data/GotchaDbContext.cs`                    |
| User entity          | `Gotcha.Core/Entities/Models/GotchaUser.cs`              |
| Login controller     | `Gotcha.Web/Controllers/HomeController.cs`               |
| Signup controller    | `Gotcha.Web/Controllers/SignUpController.cs`              |
| Logout controller    | `Gotcha.Web/Controllers/AccountsController.cs`           |
| Reset pwd controller | `Gotcha.Web/Controllers/ResetPasswordController.cs`      |
| Login view           | `Gotcha.Web/Views/Home/Index.cshtml`                     |
| Signup view          | `Gotcha.Web/Views/SignUp/Index.cshtml`                   |
| Reset pwd view       | `Gotcha.Web/Views/ResetPassword/Index.cshtml`            |
| Login VM             | `Gotcha.Web/ViewModels/HomeViewModel.cs`                 |
| Signup VM            | `Gotcha.Web/ViewModels/SignUpViewModel.cs`               |
| Seeder               | `Gotcha.Core/Data/Seeder/Seeder.cs`                      |
| Custom validators    | `Gotcha.Core/Validation/DataAnnotations/`                |

---

## Recommended Implementation Order

1. **Login (POST)** — inject SignInManager into HomeController, add POST action, wire the form
2. **Signup (POST)** — inject UserManager into SignUpController, add POST action, create GotchaUser + VipSettings, wire the form
3. **Logout** — inject SignInManager into AccountsController, call SignOutAsync
4. **[Authorize]** — protect User and Player areas
5. **Seeder passwords** — update Seeder to hash passwords for test accounts so they can log in
6. **Password reset** — token generation, validation, email (can defer email service)

Steps 1-4 are the core. Step 5 is quality-of-life for dev. Step 6 can wait.

---

## Identity Concepts Quick Reference

These are the key ASP.NET Identity services the user will work with:

- **UserManager<GotchaUser>**: create users, find by email/username, hash passwords, manage claims
- **SignInManager<GotchaUser>**: sign in (cookie-based), sign out, check passwords
- **Key methods**: `UserManager.CreateAsync(user, password)`, `SignInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure)`, `SignInManager.SignOutAsync()`
- **User.Identity.IsAuthenticated**: check if user is logged in (in controllers/views)
- **User.Identity.Name**: returns the logged-in user's UserName
- **UserManager.GetUserAsync(User)**: get the full GotchaUser from ClaimsPrincipal

---

## Notes & Decisions Log

_Space for recording decisions made during implementation sessions._

### Session 1 (2026-03-19)
- Starting point established, user wants to learn by doing (guided, not done for them)
- User is a pre-junior dev, strong frontend, new to backend and Identity
