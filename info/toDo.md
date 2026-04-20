# To Do

Overview of what still needs to be done in the Gotcha project, grouped by area.

---

## 1. Identity & Authentication (blocks most other work)

- [x] Add a migration for Identity tables (AspNetUsers, AspNetRoles, etc.)
- [x] Wire up login: HomeController needs a POST action using SignInManager to authenticate users
- [x] Wire up logout: add a logout action (probably in AccountsController) that calls SignInManager.SignOutAsync
- [x] Add `[Authorize]` attributes to the User and Player areas so anonymous users can't access them
- [x] Decide what to do with AccountsController (currently a stub with no view) — either build it out or remove it

## 2. Sign-Up Flow

- [x] Add a `[HttpPost]` action to SignUpController that creates a GotchaUser via UserManager
- [x] Convert the SignUp view to use `@model SignUpViewModel` with `asp-for` tags so server-side validation works
- [x] Handle guardian email logic in the POST action (required if under 16)
- [x] Handle profile image upload and save during sign-up (Cropper.js circle crop → 1500x1500 JPEG → ProfileImage entity in DB)
- [x] Email confirmation on signup (blocks login until confirmed)
- [x] Guardian consent email if under 16 (blocks login until guardian clicks consent link)
- [x] Client-side validation (vanilla JS — required fields, password strength, email format, age check)
- [x] Honeypot integration — reserved names/usernames and attack patterns (SQL injection, XSS) redirect to Gotcha page

## 3. Password Reset Flow

- [x] Build out ResetPasswordController: token generation, email sending, token validation, password update
- [x] Fix the "Forgot password?" link on the login page
- [x] Set up an email service (EmailService via SMTP/MailHog)

## 4. Real Data Wiring (replacing hardcoded mock data)

### User Area

- [ ] User/HomeController — load the real logged-in user's stats (games played, wins, kills, streak) and profile image
- [ ] User/GamesController Index — query the DB for the user's actual games (pending, active, ended)
- [ ] User/GamesController Create POST — create a real Game + Rules in the DB
- [ ] User/SettingsController — GET: load current user data into the form. POST: save changes
- [ ] User/StoreController — load real VipSettings from DB, wire up unlock/purchase actions

### Player Area

- [ ] Player/HomeController — resolve the actual player and game from route/session, load real data
- [ ] Player/ConfirmKillController — POST action that calls GameService.HandleValidKill or HandleInValidKill
- [ ] Player/AdminController — POST actions for: start game, remove player, promote/demote admin, validate/reject kills, update settings
- [ ] Player/SettingsController — GET: load player settings. POST: save changes

## 5. API Project

- [ ] Register DbContext and services in Gotcha.API Program.cs
- [ ] Implement all 9 API controllers (currently empty classes with no actions)
- [ ] Wire up DTOs with AutoMapper or manual mapping

## 6. Missing Game Features

- [ ] Implement AssignTargetsRandom in GameService (only circular exists, but random is documented)
- [ ] Wire up the Seeder — call Seeder.SeedAsync() in Program.cs so the DB isn't empty after migrations
- [ ] Fix JoinPlayer VipSettings issue — VipSettings navigation property needs eager loading to avoid NullReferenceException

## 7. Testing

- [ ] Add tests for custom data annotations (BirthDay, GuardianRequired, Name, Picture, UserName, UserNameOrEmail)
- [ ] Add tests for the full ImageValidationService (file extension, MIME type, magic bytes, file size — only URL check is tested)
- [ ] Add integration tests (controller tests, DB tests) — currently all tests are pure unit tests

## 8. Account Management

- [x] Privacy Dashboard (User area) — soft delete + anonymize account
- [x] Login blocks deleted accounts (IsDeleted check)
- [ ] Full data deletion flow (manual, via contact form — delete TargetAssignments → Kills → Players → User) - REVISE ??? DONT JUST DO !!! (we no obscure deleted data)

## 9. Frontend Polish

- [ ] Replace hardcoded placeholder data in User/Home view ("TheLegend27", zeroed stats, placeholder image)
- [ ] Fix the homePage.js typewriter animation to use real username data instead of hardcoded text
- [x] Profile image cropping (Cropper.js with circle crop, 1:1 aspect ratio, 1500x1500 output)

## 10. Other

- [ ] Verify EF Core JSON column storage works correctly for List<string> properties (Rules.CustomRules, Rules.KillMethods) and List<Guid> (Game.AdminIds)
- [ ] Plan for production session store (currently in-memory — swap to Redis or SQL Server for production)
- [ ] Set up proper error/exception handling middleware for production
