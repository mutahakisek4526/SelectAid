using SelectAid.Models;
using SelectAid.Persistence;

namespace SelectAid.Services;

public class AppStateService
{
    private readonly JsonStore _jsonStore;
    private readonly LogService _log;

    public AppSettings Settings { get; private set; }
    public ProfilesStore Profiles { get; private set; }
    public KeyboardLayoutsStore KeyboardLayouts { get; private set; }
    public PhrasesStore Phrases { get; private set; }
    public UserDictionary UserDictionary { get; private set; }
    public HistoryStore History { get; private set; }

    public Profile CurrentProfile => Profiles.Profiles.First(p => p.Id == Settings.CurrentProfileId);

    public AppStateService(JsonStore jsonStore, LogService log)
    {
        _jsonStore = jsonStore;
        _log = log;
        Settings = new AppSettings();
        Profiles = new ProfilesStore();
        KeyboardLayouts = new KeyboardLayoutsStore();
        Phrases = new PhrasesStore();
        UserDictionary = new UserDictionary();
        History = new HistoryStore();
    }

    public void Load()
    {
        AppPaths.Ensure();
        Settings = _jsonStore.Load(AppPaths.SettingsPath, new AppSettings());
        Profiles = _jsonStore.Load(AppPaths.ProfilesPath, new ProfilesStore());
        if (!Profiles.Profiles.Any())
        {
            var profile = new Profile();
            Profiles.Profiles.Add(profile);
            Settings.CurrentProfileId = profile.Id;
        }
        if (string.IsNullOrWhiteSpace(Settings.CurrentProfileId))
        {
            Settings.CurrentProfileId = Profiles.Profiles.First().Id;
        }
        KeyboardLayouts = _jsonStore.Load(AppPaths.KeyboardLayoutsPath, DefaultContent.CreateDefaultLayouts());
        Phrases = _jsonStore.Load(AppPaths.PhrasesPath, DefaultContent.CreateDefaultPhrases());
        UserDictionary = _jsonStore.Load(AppPaths.UserDictPath, new UserDictionary());
        History = _jsonStore.Load(AppPaths.HistoryPath, new HistoryStore());
        SaveAll();
        _log.Write("INFO", "State loaded");
    }

    public void SaveAll()
    {
        _jsonStore.Save(AppPaths.SettingsPath, Settings);
        _jsonStore.Save(AppPaths.ProfilesPath, Profiles);
        _jsonStore.Save(AppPaths.KeyboardLayoutsPath, KeyboardLayouts);
        _jsonStore.Save(AppPaths.PhrasesPath, Phrases);
        _jsonStore.Save(AppPaths.UserDictPath, UserDictionary);
        _jsonStore.Save(AppPaths.HistoryPath, History);
    }

    public void AppendHistory(string text)
    {
        History.Items.Insert(0, new HistoryItem { Text = text, Timestamp = DateTime.Now });
        if (History.Items.Count > 200)
        {
            History.Items.RemoveAt(History.Items.Count - 1);
        }
        _jsonStore.Save(AppPaths.HistoryPath, History);
    }
}
