# Claude Prompts

## prompt 201
I didn't mean console log, I meant f12 dev menu

## prompt 202
pls handle the new migration so my db is up to date

## prompt 203
yes

## prompt 204
I renamed the db to Gotcha.Db (did so in connectiostring); handle the update and new migration

## prompt 205
I created a new info folder with info about the project. I want you to check these files and see if any need updates (for example, the lobby size enum is outdated). Update correctly and tell me which files changed

## prompt 206
Implement the following plan: Clean up README.md — separate technical from non-technical

## prompt 207
save to gloal memory: use regular get {} in stead of => in Properties

## prompt 208
in the info wireframes folder, I made a new sub folder "images". Add this folder to the gitignore. Also, for every page that's in here, add a wireframe md file in /reports/wireframes

## prompt 209
add, commit and push

## prompt 210.1
in C:\Users\yolow\Desktop\eigen projecten\Gotcha\Gotcha\info\claude-info\identity there info about how I want to
implement Identity. I have started by adding AddIdentity in program.cs. Change the param values to be optimal for
safe passwords. Also change DbContext for IdentityDbContext troughout the project. When finished, list edits we will
need to do before continueing after this change. Ask more questions if needed

## prompt 210.2
Also change User (entity) to GotchaUser troughout the project

## prompt 211
I made GotchaUser inherit from IdentityUser. Do I remove the username prop (or any others) or do I keep as is?

## prompt 212
I like option 1 more. Is there any info about this problem in the files in C:\Users\yolow\Desktop\eigen projecten\Gotcha\Gotcha\info\claude-info\identity?

## prompt 213
let's go for option 1 (IdentityUser<Guid>) and change it to Guid since I like this approach way better, seems way cleaner. Note this with some info in info folder. Also update the info part of the file if needed

## prompt 214
ok, I prepared the code for the change. Can you implement it like we agreed on?

## prompt 215
UserName in IdentityUser is nullable. It shouldn't be. How should we go about that?

## prompt 216
Is there a way we could let intellisense know? It's marking User.UserName (line 30 in Player.cs) as nullable.

## prompt 217
is this the same as public override required string? UserName { get; set; } (it's ? so intellisense doesn't trip about the setter, but required to make sure it's being set).

## prompt 218
is adding required to non nullable fields in each entity a good idea?

## prompt 219
if we don't mind the work it takes to rewrite other files to make the required work. Is it better practice to add required to all non nullable properties or not?

## prompt 220
what about properties with string? Since intellisense doesn't like it rn (since not nullable but can be null technically).

## prompt 221
I don't like assigning an empty string to a required string, even if it gets handled later. Seems ugly to me. Let's not do that. Add to notes info about required in properties, why we used it in the props in GotchaUser and not in the other props or entities.

## prompt 222
can you make a new data annotation for UserNameOrEmail attribute (so in login, a user can enter both it's user or email). For the username part, I suggest looking in the UserName annotation

## prompt 223
is adding required to required fields in ViewModels a solid idea or not?

## prompt 224
in the validation service, add a new method for IsValidName. This method should also check ReservedUsernames, but it should check another array as well 'ReservedNames' where blacklisted first-and last names are kept (for example, offensive (fake) names. If there is a better way to implement this, lemme know

## prompt 225
this method would be used for both First and last names. And I would also include ReservedUsernames since nobody is named Admin or [Object, Object], so let's not allow this.

## prompt 226
when finished, make a 'Name' data annotation

## prompt 227
add IsValidBirthDay in the validator service that checks if the bday is valid, also add a BirthDay data annotation

## prompt 228
Implement the following plan: Conditional Parent/Guardian Email Field on Signup Page

## prompt 229
implement ProfileImageSource data annotation

## prompt 230
I don't understand IsAllowedImageUrl in LastLineValidationSevice.cs. Can you add more comments? (I don't get !Uri.TryCreate(url, UriKind.Absolute, out var uri) / uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps; never worked with Uri)

## prompt 231
I am checking IsAllowedImageUrl; the user should be upload to upload images from their pc locally and the server should save this. The way have it now, does it allow that or only links from online

## prompt 232
can't a user drag an image from google images to input type file technically?

## prompt 233
Suggest me the best way on how to implement validation for profile images. The end idea (not everything is in place yet) => 1) user signs up and can add an image (or do it later) and can upload an image 2) user chosen an image (either by dragging from another side (like their fb profile img) or from their pc. 3) The site cuts the image (like facebook does) so the size is correct => 4) image gets stored and used correctly. Ideally I want all possible validation during this process, starting with the upload phase

## prompt 234
let's stash the cropper idea for later, this is not our concern right now (we'll handle cropping later). Right now I wanna focus on validation

## prompt 235
Implement the following plan: Profile Image Upload Validation

## prompt 236
in site.js; for the extention methods we use HTMLElement. everywhere except in slideToggle. Why? If no reason, isn't HTMLElement correcter there?

## prompt 237
I feel like we can do the validation service better. For starters the name => ValidationService.cs, but should we also split it up or keep everything in the same service? Whatcha think?

## prompt 238
Implement the following plan: Split LastLineValidationService into 3 Focused Services

## prompt 239
create a Picture data annation for me

### prompt 239.1
it should be used when uploading an image (so in Signup, or later if they want to add / update their image after signin up), so best to check all 4. Can you also bump the max allowed file size up to 8? Also, you mentioned the tests project can't compile due to pre-existing CS9035 errors, pls expend on this

## prompt 240
update the tests so the errors are gone

## prompt 241
so ProfileImageSource data annotation can be removed? (and replaced with PictureAttribute)?

### prompt 241.1
I would like a combo tbh. I don't care where the image comes from when uploaded (local pc or internet) as long as it's clean

## prompt 242
GuardianEmail can't be null if the user is too young. Should we do this with data annotations and/or other ways as well or do we just handle this in the controller (what I currently have in mind). (I will do it in controller anyways, dw, like I will do with all annotations as well (extra sure). What about this?

## prompt 243
yes, make a GuardianRequired data annotation. Would I still need to use [EmailAddress(ErrorMessage = "Please enter a valid email address!")] at the property (and add GuardianRequired) or will it include the check? (currently I am thinking of 2 different data annotions, do you agree?)

## prompt 244
stash the first 200 logs in the archive (C:\Users\yolow\Desktop\eigen projecten\Gotcha\Gotcha\info\archive\Claude_Prompts_Archive.md) and update the Claude.md files where needed

## prompt 245
in the signup page, the guardianEmailSection shouldn't have an id (due to the framework, will give issues later); it doesn't look like the other input fields (give same styling) and it doesn't automatically show when a birthday under 16 years old is select in Birthday (handle via js). Pls fix

## prompt 246
it's almost fine but border: 2px solid var(--secondary-color-blue); doesn't get applied on the new guardian input field. (site.css, line 323 and signup index cshtml)
