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

## prompt 81
fix the errors, fix possible improvements 3 and 4 (ignore 1 and 2, those are fine), I like the current UI above the '-' since these settings may be turned off so it will never need to show data, so '-' is overkill

## prompt 82
add, commit and push changes

## prompt 83
save to memory

## prompt 84
save this to memory: If you ever need extra info (cuz I wasn't clear enough), need me to do something (like download something you need) or you have doubts abt how to tacke or a problem or anything like that; inform me before continuing. You don't start tasks unless everything is fully clear and you are fully set unless I say otherwise

## prompt 85
go over the project and save to memory what you still need to save

## prompt 86
Create the new Project for UnitTests with the necessary Unit Tests in it. Write the Unit Tests files, as in, write in comments what needs to be checked and what we expect (like normal) and make the methods, but give em all a return NotImplementedException() since I want to write them myself to learn; but I want you to prepare this for me

## prompt 87
if I add new entities, services or methods elsewhere that needs tests, create the Unit Tests files (if needed) and prep the methods without writing like you did now. Tell me in this console you created a new Unit Test when this happens. Log this so you will remember this within this project only

## prompt 88
save this to your general memory (across all future projects) if there is a Claude_Prompts.md file present I want you to log the prompts like you are doing in this project. If there is no Claude_Prompts.md file you don't have to log

### prompt 88.1
yes

## prompt 89
also save to general memory CLAUDE.md to follow the rules about code readability

## prompt 90
create the view (and viewModel and edit the controller) of the confirmKill page. It should have the option to confirm either that you killed the target or that you got killed. If assassin mode is enabled they should also be able to confirm that they killed their killer. Keep the style similar to other views.

## prompt 91
implement (and if needed, correct) all unit tests

## prompt 92
add, commit and push changes

## prompt 93
create the games view and viewmodel (and edit the controller as needed). It should list all games in 2 categories, active games and ended games. It should show the games as a list with info in it (like I did in player home). Info should depend on the game. When the user clicks on it they should be taken to the player home page of that game

### prompt 93.1
startdate, endate (if finished), winner (if finished), player count, alive (yes or no) (if not finished)

## prompt 94
edit the new view, I want it all on 1 line like a table. More like the Kills Overview part of player index

## prompt 95
make the text inside the tables (not alive) primary blue var color (or a similar one with high contrast) in stead of black

## prompt 96
check the log and update if needed

## prompt 97
don't make em sub prompts, they are all seperate prompts. Also don't forget to keep logging during the project

## prompt 98
create the view for player settings. It should be similar to user settings. Here the player should be able to change their username and image for that game.

## prompt 99
also create the viewmodel and edit the controller like last tasks

## prompt 100
fix the title in Target Info and Confirm kill to be uniform with the others

### prompt 100.1
Player Home title

## prompt 101
add an info icon to the player settings title like I did in signup. When clicked it should show a modal (like in signup) with the text "These settings are only for the currently selected game and won't affect your profile outside of this game." (or something similar if it's better). Do the same for user settings (but with a correct modal text of course). so the user has a clear understanding of the point of the different setting pages

## prompt 102
add, commit and push changes

## prompt 103
Implement the following plan: Player Admin Page (full plan with pre-game/during-game sections, viewmodels, controller mock data, toggle switches, tables, VIP-gated settings)

## prompt 104
I don't like that the script is in the view, give it its own js file and link it as we did in the other views

## prompt 105
in the new admin view, add an info icon to actions that will make the modal show with info that if you click image or username, you will send a request to the player to change their image or username. Also create another modal click event on the buttons for image and username to ask for confirmation 'are you sure...' to make sure players don't get requests to change their image or username by accident

## prompt 106
make the text input fields backgrounds transparant like we did with other fields (take signup page for example). also change the color of the placeholder text to be uniform.

## prompt 107
the info icon for actions isn't properly placed. Check signup page as example on how to place it. also, add options to all settings (also the paid ones). We will block the toggle later on if they don't have the correct plan and tell them to buy the plan first. So the settings should contain all settings.

## prompt 108
I don't like @* use HTML comments instead

## prompt 109
the admin view is missing a seperation line between custom rules and custom kill methods. The line between custom kill methods and kill methods needs to be removed. A line between kill methods and assassin mode needs to be added. the line between chaos mode and chaos timer needs to be removed. A line between chaos timer and timed kills needs to be added. The line between timed kills and Target Timeout needs to be removed.

## prompt 110
the labels are missing for and are invalid in HTML (won't cause errors, but we like to work correctly)

## prompt 111
won't the id's cause problems with the framework later or is it fine?

## prompt 112
in the player admin view, why do we check if the game has started and show a whole section based on that in stead of making 1 section and just hiding / showing the settings based on if the game has started. If this is a possible solution, do it that way

## prompt 113
Implement the following plan: Shop (Unlock Features) View & ViewModel

## prompt 114
the actions info icon on the admin page is still wrong. Its too big and right next to the actions, it should be on the right above. check the signup page for examples

## prompt 115
its worse now, now it just moved way more rigth. Put it back in place correctly

## prompt 116
still wrong. Try to tackly it the same way you did with the titles in the settings pages

## prompt 117
now its underneath actions

## prompt 118
nearly there, add margins margin: 0 0 1.5vh 0.5vw !important; to the info icon

## prompt 119
in index.cshtml I see you used onclick in the buttons. I don't like this. Use click events in the js file (and save this in your general memory)

## prompt 120
<button class="btn notifyBtn notifyImageBtn col-12 col-lg-5" title="Notify to upload image" data-player="@player.Name" data-type="image">
<i class="bi bi-image"></i> Image
</button>
<button class="btn notifyBtn notifyUsernameBtn col-12 col-lg-5" title="Notify to set username" data-player="@player.Name" data-type="username">
<i class="bi bi-person"></i> Username
</button>
what are those i's doing in there?

## prompt 121
in the admin page, depening on if the mode is unlocked the checkbox should get the disabled tag. The input field for kill methods should be disabled by default; but enabled when custom kill methods gets enabled

## prompt 122
the input fields for target timeout and chaos timer should also be disabled and enabled if the switch gets turned on

## prompt 123
on the store page, the plans have buttons. Can you make sure they are all aligned horizontally? They standard plan button is currently higher since the text isn't the same

## prompt 124
can you add in info icon to Game Features, Lobby Size and subscription plans like we did before. When clicking Game Features and Lobby size info icons it should show a modal that tells the user that these unlocks are permanent and will unlock these permanently to their account. If they create a new lobby they will be able to turn these on and all players in that game will be able to play with those settings for free (only the creator needs it). Same goes for lobby size. Subscription plans modal should inform the user about the subscription plans

## prompt 125
save to general memory; when making text that the user can see, for example modal texts etc, make the text look human. For example, avoid - etc

## prompt 126
on some pages, when opening the phone nav menu by clicking the harburger icon, it will open the nav menu but won't hide the page. Fix this pls

## prompt 127
check all views and remove redundant pageToHide wrappers

## prompt 128
I notice some views have 2 modals both for showing dialog. I prefer having 1 modal (if it's just for the purpose of showing info) and modifying the info in the modal itself via js. (log everything in this prompt till now (not next info) in general Claude.md. => for this project, you can reference the signup page on how I did it there

## prompt 129
yes, do it where it's needed

## prompt 130
add styling in site.css for all modals so that they look better, more in line wit the general vibe of the site.

## prompt 131
in the admin page, before the game starts admins should also be able to remove players (also with confirmation modal ofc)

## prompt 132
the buttons for image, username and remove on the admin page should all become lowercase; 0 margin; col 3 in stead of 4 (for lg)

## prompt 133
image and username should be col-4 and remove col-3 (on lg) in admin page

## prompt 134
give the buttons image, username and remove in the admin page 0.5vh 0 0.5vh 0 margin in stead of 0 and make padding 0.6vh 0;

## prompt 135
in target info, under time left, add a field where the player can see custom game rules if there are any. Also edit the viewmodel and controller for this

## prompt 136
add a line above the new game rules section; also redo the section. I want Game Rules: to be above the game rules text and the game rules text should have a border and background.

## prompt 137
game rules are seperated by commas, so in stead of string? CustomRules should be List<string>? throughout the project so we can list them nicely in the player view. pls do this

## prompt 138
in player home, game rules. The the game rules are currently black. Can you make em primary color to go along with the project (or a similar colors with higher contrast)

## prompt 139
if assassin mode is on for that game, it should also list "attempting to kill your hunter is allowed. If you guess wrong and attempt to kill the wrong killer you will die in stead." (or something similar if better). If other game modes (like chaos mode) are turned on, it should also list that with added info (like if timed out, what timespan players have for each kill)

## prompt 140
change minutes to hours in admin; also chaos mode timer now has 1 value, make this 2 (between x and y) so its random. We will also need to update the rules entity for this and the viewmodel.

## prompt 141
at the games pages for the user, add a "new game" button in the same style of the start game button on the admin page. Place the button between the active games card and the ended games card. Give it a bit of margin top and more margin bottom.