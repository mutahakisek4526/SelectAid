using System.IO;

namespace SelectAid.Persistence;

public static class AppPaths
{
    public static string Root => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SelectAid");
    public static string SettingsPath => Path.Combine(Root, "settings.json");
    public static string ProfilesPath => Path.Combine(Root, "profiles.json");
    public static string KeyboardLayoutsPath => Path.Combine(Root, "keyboardLayouts.json");
    public static string PhrasesPath => Path.Combine(Root, "phrases.json");
    public static string UserDictPath => Path.Combine(Root, "userDict.json");
    public static string HistoryPath => Path.Combine(Root, "history.json");
    public static string BackupsDir => Path.Combine(Root, "backups");
    public static string LogPath => Path.Combine(Root, "log.txt");

    public static void Ensure()
    {
        Directory.CreateDirectory(Root);
        Directory.CreateDirectory(BackupsDir);
    }
}
