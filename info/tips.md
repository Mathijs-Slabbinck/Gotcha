# Tips for Working with Claude Code

## 1. Batch related tasks into one prompt
Instead of 5 separate prompts like "fix the margin", "also the padding", "also make it red", "also on mobile" — combine them into one.
Each prompt costs a full context reload. One detailed prompt = one pass = faster and cheaper.

## 2. Reference files and lines when describing issues
"The button in `Admin/Index.cshtml` line ~85 has wrong margin" is way faster than "the button on the admin page has wrong margin".
The less searching Claude does, the more doing it does.

## 3. Use screenshots for UI issues
When something looks wrong visually, paste a screenshot. (drag it in the Claude Powershell window)
Claude can read images. "This looks wrong [screenshot]" beats a paragraph explaining what's visually off.

## 4. Front-load context in big prompts
*Use plan-mode for bigger features/fixes that involve editing/creating multiple files in your project*
When asking for a new feature, give everything upfront: what it should look like, where it goes, how it behaves, edge cases.
The "implement the following plan" approach works great for this.

## 5. Say "don't ask, just do it" when confident
The default is "always ask first", which is smart.
But for tasks where you already know exactly what you want, adding "just do it" or "no questions" saves a full round-trip.

## 6. Use /commit shorthand
Instead of "add, commit and push changes" just say `/commit`. Saves typing and parsing.

## 7. Keep CLAUDE.md files updated when conventions change
*Make claude follow conventions/rules over all projects.*
If a new pattern is decided mid-session (like avoiding `??`), update the global CLAUDE.md right then (Claude has been configured so it will auto update the correct memory files and agents).
Future sessions pick it up automatically instead of needing a reminder.

## 8. Use "fix all" instead of one-at-a-time
When Claude lists multiple issues (e.g. "5 things to improve"), saying "fix all" or "fix 1, 3, and 5" in one prompt is faster than going through them one by one.

## 9. Pin known-good examples
Pointing to a reference file for style or pattern (e.g. "check the signup page for reference") is faster than describing the pattern from scratch.

## 10. Avoid re-explaining things that are in CLAUDE.md
Now that conventions are in the config files, no need to say "make it readable" or "no ternary operators". Save prompt space for new information.

---

**Biggest wins: #1 (batching) and #4 (front-loading context).** Most extra round-trips come from drip-feeding requirements across 3-5 follow-up prompts. One well-structured prompt replaces all of them.
