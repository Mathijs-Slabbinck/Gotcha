# Entities

## VipSettings
- Guid Id
- bool AssassinModeUnlocked
- bool ChaosModeUnlocked
- bool TimedKillsUnlocked
- bool CustomKillMethodsUnlocked
- MaxLobbySize MaxLobbySize
- Plan UserPlan

## User
- Guid Id
- string Firstname
- string Lastname
- string Username
- string Email
- Genders gender
- string? ProfileImageSource
- DateTime BirthDate
- DateTime AccountCreationDate
- List\<Player> PlayerAccounts
- VipSettings VipSettings (which premium settings do they have unlocked (when they start a new game they will be able to use the settings they unlocked). Also contains the UserPlan.

## Player
- Guid Id
- User user
- Game game
- string username
- string? ProfileImageSource
- bool IsAlive (We can also get it via a method by checking the targetAssignments)
- bool IsAdmin
- bool IsSpectator
- string Notes (allows users to store custom notes during the game)
- ICollection\<TargetAssignment> TargetAssignments

## TargetAssignment
- Guid Id (because it can be invalid and the 2 other keys aren't always unique combinations)
- Player Target
- Player Hunter
- DateTime TargetAssigned
- DateTime? AssignmentFinished
- DateTime? AssignmentExpirationDate
- Kill? Kill
- string? Weapon (in case custom weapons are on)
- AssignmentStatus assignmentStatus

## Kill
- Guid Id
- Game game
- Player Killer
- Player Victim
- DateTime Moment
- string? Weapon
- string KillMessage ($"{killer} has killed {victim} on {localTime.ToString("yyyy'-'MM'-'dd' - 'HH':'mm'")})
- TimeSpan TimeSinceAssignedTarget
- string Reason (by default this is just the KillMessage, but this can also be the reason it was voided (for example, "Targets were re-assigned for chaos mode"))
- private bool IsValid

## Game

### Fields
- Guid Id
- string Name
- DateTime CreationDate
- DateTime? StartDate
- DateTime? EndDate
- bool HasStarted (could be done without it by checking StartDate)
- bool IsFinished (could be done without it by checking EndDate)
- Guid? WinnerId (Winner is a computed property from Players)
- List\<Guid> AdminIds (Admins is a computed property from Players)
- int MaxPlayers (also could do without this)
- Guid CreatorId (Creator is a computed property from Players)

### Methods
- JoinPlayer(Player player, bool isAdmin = false)
- HandleValidKill(Player killer, Player victim, string? weapon = null)
- HandleInValidKill(Player killer, Player victim, string? reason = null, string? weapon = null, AssignmentStatus assignmentStatus = AssignmentStatus.Failed)
- StartGame()
- GetLivingPlayers()
- GetEliminatedPlayers()
- GetDisputedKills()
- AssignTargets()

## Rules
- Guid Id
- bool IsAssassin
- bool ShowHunter
- bool ShowPlayerImages
- bool ShowGender
- bool EnforcePlayerImages
- bool ShowRealNames
- bool ShowUsernames
- bool ShowLivingPlayerCount
- bool ShowLivingPlayerNames
- bool ShowLivingPlayerNamesToDeath
- bool IsTimed
- TimeSpan TargetTimeOut
- List\<string>? CustomRules
- bool CustomKillMethods
- List\<string>? KillMethods
- bool IsChaos
- TimeSpan ChaosTimerMin
- TimeSpan ChaosTimerMax
- TimeSpan KillConfirmationTimer

## Notes
- User is the main profile; for each game a user joins, we create a player. Note that both have a username field; this is because the user can set a different username for each game. By default, the default username (from the User) is used.
- Users will be able to 'buy' settings. For example, a user can choose to unlock 'Assassin GameMode' for $3 and unlock the gamemode permanently to their account. When they start a game they will be able to turn this setting on and every player that joins that game will be able to benefit from the new setting.
- Users can also opt to subscribe to plans depending on their group size. A plan with all settings unlocked for, for example, 50 players max would be $1/week or $3/month.
