# Claude Prompts

## prompt 1
https://claude.ai/share/8a9ed166-1da5-477c-b3ba-fd03036e8edb https://github.com/users/Mathijs-Slabbinck/projects/4 I want you to help me with this project. I am a student (pre junior) dev. I have mastered front end since this is my passion, I am pretty new to backend. I want you to be me copilot in this project. Don't do the work for me but let me know where and how to improve my app. I value code readability since it's easier to debug and read. The core values should be 1) safety (this is my first project, that means I should take extra steps (as much as possible) towards safety (no data leaks etc)) 2) maintaince 3) performance
Always take safety over performance or maintance.
Guide me as well as you can; act like a senior dev that is doing the work for me or telling me what to do but helps to explore my ideas and guide me in the process

## prompt 2
Ask me everything you need to know. More info: I am a student Graduate programmer at Howest, I have system and software IT in high school. I have been coding front end between that. Howest has partnered with Microsoft so we use .Net and Microsoft powered tools and frameworks by default. We learned MVC and databases and I can use link and use a database (CRUD) (a local one but that's a problem for later).  I know they will teach us in the Maui framework so my first build is going to be in that since it's a good way to practice. I am using Visual Studio (ASP.NET Core Web App (MVC), nullable disabled, no AI powered features. I just made the first version of the enitities but I am still reworking and making sure they are fully finished, foolproof and can be counted on for the rest of the project. My main concern is learning; I am not sure if I will release this version (I am think of rewriting it in react later but I need to learn Maui now) but I wanna do it in a way where I teach myself right and where it is safe and could be pushed online if it ends up being a solid working project. Make sure to ask me more questions if you need more info; best to get us on a straight line before heading in and tackling the project.

## prompt 3
Ask me everything you need to know. More info: I am a student Graduate programmer at Howest, I have system and software IT in high school. I have been coding front end between that. Howest has partnered with Microsoft so we use .Net and Microsoft powered tools and frameworks by default. We learned MVC and databases and I can use link and use a database (CRUD) (a local one but that's a problem for later).  I know they will teach us in the Maui framework so my first build is going to be in that since it's a good way to practice. I am using Visual Studio (ASP.NET Core Web App (MVC), nullable disabled, no AI powered features. I just made the first version of the enitities but I am still reworking and making sure they are fully finished, foolproof and can be counted on for the rest of the project. My main concern is learning; I am not sure if I will release this version (I am think of rewriting it in react later but I need to learn Maui now) but I wanna do it in a way where I teach myself right and where it is safe and could be pushed online if it ends up being a solid working project. Make sure to ask me more questions if you need more info; best to get us on a straight line before heading in and tackling the project.

## prompt 4
I was gonna do Model Validation attributes + EF Core standard + Controller checks + front end security (doesn't really help but still) + enitity checks and extra checks using that ThirdLineValidationService

## prompt 5
That's where it is, on entity level in the business logic. Its the very very last line after all checks have already passed.  Could it be usefull to improve the service or is it best to remove it (except the email). I would use ValidateAntiForgeryToken, ModelState.IsValid, [Required] etc since we have also learned all of this.

## prompt 6
// Sanitize input for XSS and basic SQL injection public static string SaniziteInput(string input) { if (string.IsNullOrEmpty(input)) { return input; } // HTML encode to prevent XSS // &copy; => ©; &amp; => &; &lt; => <; &gt; => > ... string sanitized = WebUtility.HtmlEncode(input); // If there are any remaining raw HTML tags, delete them sanitized = Regex.Replace(sanitized, @"<.*?>", string.Empty); if(!IsInputClean(sanitized)) { throw new ArgumentException("Input still contains potentially dangerous content after sanitization!"); } return sanitized; }
public static bool IsValidLength(string input, int minLength, int maxLength) { if (string.IsNullOrEmpty(input)) return false; return input.Length >= minLength && input.Length <= maxLength; }
public static bool IsCleanProfileImageSource(string? profileImageSource) { // Null or empty is considered clean if (string.IsNullOrEmpty(profileImageSource)) { return true; } // Check for valid URL format (basic check) if (!Uri.TryCreate(profileImageSource, UriKind.Absolute, out var uriResult) || uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps) { return false; } return IsInputClean(profileImageSource); }
public static bool IsValidEmail(string email) { if (string.IsNullOrWhiteSpace(email)) return false; // More robust regex pattern string pattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$"; if (!Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase)) { return false; } // Additional validation using MailAddress to catch edge cases try { var addr = new System.Net.Mail.MailAddress(email); return addr.Address == email; } catch { return false; } }
public static bool IsInputClean(string input) { if(input.Equals("null", StringComparison.OrdinalIgnoreCase) || input.Equals("void", StringComparison.OrdinalIgnoreCase) || input.Equals("undefined", StringComparison.OrdinalIgnoreCase) || input.Equals("NaN", StringComparison.OrdinalIgnoreCase) || input.Equals("[Object Object]", StringComparison.OrdinalIgnoreCase) ) { return false; } return true; }
What about changing it to this? (Does .Net also check if emails are valid and if images are clean or not, if they do those can be deleted, same with HTML sanitization (can't hurt to have it, > remains >, but if it auto happens it's a bad idea)

## prompt 7
IsInputClean, I have this because I don't want users with usernames like [Object Object] or void or so in my database as I don't want unneeded headaches later

## prompt 8
/init

## prompt 9
Add claude.md file

## promp 10
git push

## prompt 11
Please review the .core and tell me what can be improved or won't work

## prompt 12
Fix GetAllKills() and GetAllDeaths()

## prompt 13
I changed the AttackerRepoService messages in the Errors, can you do the other files where it’s needed the same way.

## prompt 14
now move the {id} in the first message in the repo services in the Error entities to the 2nd message like I did in the AttackerRepoService

## prompt 15
Finish the seeder for me

## prompt 16
I’m guessing it’s better to get all logic out of the entities.

## prompt 17
What about the Game entity? (and it’s methods)

## prompt 18
I now have a GameRepoService, best to move em here?

## prompt 19
Start by creating the GameService file first

## prompt 20
yes

## prompt 21
Best to simplify even further or not?

## prompt 22
yes

## prompt 23
Now can’t we get rid of the constructors?

## prompt 24
Yes, with get; init; to lock it down

## prompt 25
Update the User entity the same way

## prompt 26
Now TargetAssignment entity

## prompt 27
Clean up the remaining entities (Kill, Player, Rules, Log, Attacker)

## prompt 28
/upgrade

## prompt 29
Update seeder now

## prompt 30
You mentioned GameService needs updates, lets fix that first

## prompt 31
Keep this in mind for the rest of the project: I can use AI for this but I will need to present it and may need to explain code on the spot. I want it to be super readable. I refactored GetMaxPlayersForLobbySize and CreateKill in GameService as example. Try to avoid lambda, I can read it so if it’s clearly best it’s fine but I prefer simple if else. Use the simple switch statement etc etc. Readability is important

## prompt 32
Now save these learnings to memory.md

## prompt 33
Go trough the core project and see what needs improvement or fixing

## prompt 34
Fix them in order

## prompt 35
Lets rewrite the username validation; it’s way to complex. Remove it from the config and make a simple list check in the validation service.

## prompt 36
Recheck the core app to see if we missed anything or anything can use improvement. Also look for code that could be written better (readability). I need to be able to explain all code on the spot under stress.

## prompt 37
yes

## prompt 38
Save the next to your memory :try to avoid ?? where you can as well

## prompt 39
Go trough the .Web project and see what needs improvement

## prompt 30
yes

## prompt 31
Save to memory and commit

## prompt 32
push it

## prompt 33
add-migration "InitialMigration"
Build started...
Build succeeded.
The pipeline has been stopped.
PM> add-migration "InitialMigration"
Build started...
Build succeeded.
The running command stopped because the preference variable "ErrorActionPreference" or common parameter is set to Stop: C:\Users\yolow\Desktop\eigen projecten\Gotcha\Gotcha\Gotcha.Web\Gotcha.Web.csproj : warning NU1510: PackageReference Microsoft.Extensions.Configuration.Json will not be pruned. Consider removing this package from your dependencies, as it is likely unnecessary.


## prompt 34
Add-migration « InitialMigration »

## prompt 35
stop

## prompt 36
git pull

## prompt 37
I did the pull, it should be fixed now. Check the web project again pls.

## prompt 38
yes

## prompt 39
Save to memory and commit

## prompt 41
Now recheck the js files for improvements

## prompt 42
yes

## prompt 43
commit and push

## prompt 44
Since the last refactors we did the navbar in _layout (the regular one, not in area) got messed up. Can you check all classes you changed in html tags, if they add margin or padding (if it was a typo that did nothing and now it does it added margin or padding) and list them for me pls

## prompt 45
Create a new view for logged in users (inside the area) for the user settings (settings.cshtml). The user should be able to change their firstname, lastname, username, image, birthday and email. There should also be a link to the userSettings page. Make the view in the same style as the others. Take note of the signup page since that one is most similar.

## prompt 46
I made a new word doc in the project. This document lists all prompts in this chat. When I give you a new prompt; can you add it to the word file automatically so I have a clean log of all prompts?

## prompt 47
Switch to md

## prompt 48
I made the new md file myself (Claude prompts.md). Can you use this file to log all prompts (including this one) so I have a clear overview of all prompts used in this project

## prompt 49
rename it to Claude_Prompts

## prompt 50
save to your memory that you have to log every prompt in that file in this project

## prompt 51
in the settings page the bday input field placeholder is white. it should be same blue as the others, untill a date is entered (then it turns white) (like in signup) fix this pls

## prompt 52
the view needs 2 links, 1 to resetPassword (don't add the asp-link yet, this will send an email first) and a link to the "Unlock Features (shop)" page (doesn't exist yet, so don't add the asp-link itself yet)
### prompt 52.2
pick whatever is most professional given the context

## prompt 53
when you ask for sub-prompts (like Where should the two links go — below the Save button, or somewhere else?) also log the answer prompts in the Claude-Prompts file like I did right now.
*(copy pasted 52 and 52.2 for context here)*

## prompt 54
I worked a bit on the links, I gave a good idea of what I want it to look but the 2nd link has different margin/padding than the first. Can you find the cause and fix it

## prompt 55
_layout for example has it's css file directly attached to the html file. I have them seperate (in the css folder). I am guessing having them attached is the cleaner option. How to do this?
### prompt 55.2
I mean like this (Image attached: _Layout.cshtml with _Layout.cshtml.css next to it)

## prompt 56
do this for all other views

## prompt 57
I am guessing the same goes for js files? If yes, can you also do that for me

## prompt 58
currently the idea with the areas is the following: standard (no area) for unauthenticated users, then an area for authenticated users (User) and an area another area for authenticated users that are in-game (Player) (all 3 need a different nav bar). Is this a good idea/setup or do you have better ideas/suggestions/setups to achieve a clean architecture

## prompt 59
I am thinking, players that are dead (but also in game) should also see a different navBar. How would I best tackle this in my architecture

## prompt 60
can you do this for me

## prompt 61
can you fix the homecontroller in player area so it will show the correct layout

## prompt 62
InvalidOperationException: Unable to resolve service for type 'Gotcha.Core.Services.Repository.UserRepoService' while attempting to activate 'Gotcha.Web.Controllers.SignUpController'.

## prompt 63
add and commit the changes

## prompt 64
How would I add "admin" to the navbar? Could I write logic for this in _layout or this is bad practise?

## prompt 65
I wanna do the player _layout with 1 _layout file using partials. I have already created the files. Could you implement it this way?

## prompt 66
I have 3 files with the same layout, just a different navbar (and area). Is there a clean way to use 1 _Layout and use partials for loading them in or is this bad practise given the areas

## prompt 67
I enabled nullable. This shouldn't be an issue since I used ? to define nullable vars all over but can you check to be sure

## prompt 68
I am working on the player home, now we use ViewData for things like IsAdmin, IsAlive. Can you check the ViewModel I made so far and see if we should move that logic there (I am thinking not because it affects the navbar and thus also other views so may be good to keep it separate). Also, I should use the ViewModel instead of ViewData for the rest right?

## prompt 69
ArgumentOutOfRangeException: Token 2000000 in player home, why? It works if I just do simple p tags in the first if, but I want the block I added (but this causes the error).

## prompt 70
yes fix it

## prompt 71
add, commit and push the changes

## prompt 72
my css in Index.cshtml.css in player home for the targetImage doesn't work. When I do h1 color white it works so the file is linked correctly but I can't seem to target the image (also not by doing .glowingCard img or so)

## prompt 73
I just added a timer to the player home index view. Add a js script that will make it count down visually for the user in a correct tempo

## prompt 74
in the player home view, the blocks with class statBlock in the in the first card (Current Target) can't seem to target their css (no border-bottom in shown). I also tried ::deep but still nothing. Can you suggest a fix

## prompt 75
apply the fix without breaking or changing how it looks for the user

## prompt 76
the 2 cards on the player home don't have the same with but they should have. Find out why and suggest a fix

## prompt 77
apply the fix

## prompt 78
when scrolling, the navbar image is sticky (stays on the page, the nav itself scrolls away). I am pretty sure this is because of the position absolute but it has to be absolute. Present a fix that won't break the navbar and the logo (and it's click event where it 'claps out')

## prompt 79
yes, apply the fix

## prompt 80
I think I finished the home view for players (including viewmodel (and added test data in the controller). Can you check out this page; see if everything is done correctly. Let me know what can use improvement. Look for the next 3 things: errors / bugs | possible improvements (possible extra data I didn't think of etc) | possibe UI improvements if there are any (as in, things that could be in a better order or easier on the eye). It's ok to say everything is ok if it is. List me the suggestions per category when done