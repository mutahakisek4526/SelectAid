using SelectAid.Models;
using SelectAid.Services;

namespace SelectAid.ViewModels;

public class SettingsViewModel : ViewModelBase
{
    private readonly MainViewModel _main;
    private readonly AppStateService _state;
    private readonly StartupService _startup;
    private readonly SpeechService _speech;

    public SpeechSettings Speech => _state.CurrentProfile.Speech;
    public AppSettings AppSettings => _state.Settings;

    public bool AutoStartEnabled
    {
        get => AppSettings.AutoStartEnabled;
        set
        {
            AppSettings.AutoStartEnabled = value;
            _startup.SetEnabled(value, Environment.ProcessPath ?? string.Empty);
            _state.SaveAll();
            RaisePropertyChanged();
        }
    }

    public RelayCommand SaveCommand { get; }
    public RelayCommand BackCommand { get; }

    public SettingsViewModel(MainViewModel main, AppStateService state, StartupService startup, SpeechService speech)
    {
        _main = main;
        _state = state;
        _startup = startup;
        _speech = speech;
        SaveCommand = new RelayCommand(_ => Save());
        BackCommand = new RelayCommand(_ => _main.NavigateTo(nameof(HomeViewModel)));
    }

    private void Save()
    {
        _speech.Configure(Speech);
        _state.SaveAll();
    }
}
