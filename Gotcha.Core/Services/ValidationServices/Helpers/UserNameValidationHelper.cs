using Microsoft.Extensions.Configuration;

internal class UserNameValidationHelper
{
    private readonly IConfiguration _configuration;
    private HashSet<string> _reservedUsernames;

    internal UserNameValidationHelper(IConfiguration configuration)
    {
        _configuration = configuration;
        LoadReservedUsernames();
    }

    private void LoadReservedUsernames()
    {
        var usernames = _configuration.GetSection("UserSettings:ReservedUsernames").Get<List<string>>() ?? new List<string>();

        _reservedUsernames = new HashSet<string>(usernames, StringComparer.OrdinalIgnoreCase);
    }

    internal bool IsReservedUsername(string username)
    {
        return _reservedUsernames.Contains(username?.Trim() ?? string.Empty);
    }

    internal HashSet<string> ReservedUserNames
    {
        get { return _reservedUsernames; }
    }
}