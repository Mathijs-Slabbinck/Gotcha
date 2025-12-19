using Microsoft.Extensions.Configuration;

public class UsernameValidationService
{
    private readonly IConfiguration _configuration;
    private HashSet<string> _reservedUsernames;

    public UsernameValidationService(IConfiguration configuration)
    {
        _configuration = configuration;
        LoadReservedUsernames();
    }

    private void LoadReservedUsernames()
    {
        var usernames = _configuration.GetSection("UserSettings:ReservedUsernames").Get<List<string>>() ?? new List<string>();

        _reservedUsernames = new HashSet<string>(usernames, StringComparer.OrdinalIgnoreCase);
    }

    public bool IsReservedUsername(string username)
    {
        return _reservedUsernames.Contains(username?.Trim() ?? string.Empty);
    }

    public HashSet<string> ReservedUserNames
    {
        get { return _reservedUsernames; }
    }
}