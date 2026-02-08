using SelectAid.Models;

namespace SelectAid.Services;

public class MigrationService
{
    private const int CurrentVersion = 1;

    public void Migrate(AppSettings settings, ProfilesStore profiles)
    {
        if (settings.SchemaVersion <= 0)
        {
            settings.SchemaVersion = 1;
        }

        foreach (var profile in profiles.Profiles)
        {
            if (string.IsNullOrWhiteSpace(profile.ThemeId))
            {
                profile.ThemeId = "Friendly";
            }
        }

        settings.SchemaVersion = CurrentVersion;
    }
}
