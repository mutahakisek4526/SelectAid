using System.Collections.ObjectModel;
using System.Windows.Input;
using SelectAid.Input;
using SelectAid.Models;
using SelectAid.Services;
using SelectAid.Overlay;

namespace SelectAid.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly AppStateService _state;
    private readonly SpeechService _speech;
    private readonly OverlayService _overlay;
    private readonly InputRouter _inputRouter;
    private readonly Scan.ScanEngine _scanEngine;
    private readonly BuzzerService _buzzer;
    private readonly StartupService _startup;
    private readonly PcControlService _pcControl;
    private readonly PredictionService _prediction;
    private readonly BackupService _backup;
    private readonly LogService _log;

    public NavigationService Navigation { get; }
    public ViewModelBase CurrentView => Navigation.CurrentView;
    public Profile CurrentProfile => _state.CurrentProfile;
    public bool IsPaused { get; private set; }

    public MainViewModel(AppStateService state, SpeechService speech, OverlayService overlay, InputRouter inputRouter,
        Scan.ScanEngine scanEngine, BuzzerService buzzer, StartupService startup, PcControlService pcControl,
        PredictionService prediction, BackupService backup, LogService log)
    {
        _state = state;
        _speech = speech;
        _overlay = overlay;
        _inputRouter = inputRouter;
        _scanEngine = scanEngine;
        _buzzer = buzzer;
        _startup = startup;
        _pcControl = pcControl;
        _prediction = prediction;
        _backup = backup;
        _log = log;

        var home = new HomeViewModel(this);
        var aac = new AacViewModel(this, _state, _speech, _prediction, _buzzer);
        var phrases = new PhrasesViewModel(this, _state, _speech);
        var layouts = new KeyboardLayoutsViewModel(this, _state);
        var overlayPanel = new OverlayPanelViewModel(this, _state);
        var settings = new SettingsViewModel(this, _state, _startup, _speech);
        var supporter = new SupporterViewModel(this, _state);
        var training = new TrainingViewModel(this);
        var logs = new LogsViewModel(this, _log);
        var backupRestore = new BackupRestoreViewModel(this, _backup, _state, _log);

        Navigation = new NavigationService(home);
        Navigation.Register(aac);
        Navigation.Register(phrases);
        Navigation.Register(layouts);
        Navigation.Register(overlayPanel);
        Navigation.Register(settings);
        Navigation.Register(supporter);
        Navigation.Register(training);
        Navigation.Register(logs);
        Navigation.Register(backupRestore);
        Navigation.CurrentViewChanged += () => RaisePropertyChanged(nameof(CurrentView));

        _overlay.HomeRequested += () => NavigateTo("HomeViewModel");
        _overlay.PauseRequested += TogglePause;
        _overlay.MouseGridRequested += ShowMouseGrid;
        _inputRouter.InputReceived += OnInputReceived;
    }

    public void NavigateTo(string viewModelName) => Navigation.NavigateTo(viewModelName);

    public void ShowOverlay() => _overlay.Show(_state.CurrentProfile);

    public void CloseOverlay() => _overlay.Close();

    public void ShowMouseGrid()
    {
        if (!_state.CurrentProfile.PCControl.MouseGridEnabled)
        {
            return;
        }
        var grid = new MouseGridWindow(_state.CurrentProfile.PCControl.MouseGridDivisions);
        grid.Show();
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;
        _inputRouter.SetPaused(IsPaused);
        RaisePropertyChanged(nameof(IsPaused));
    }

    public void HandleKey(Key key) => _inputRouter.HandleKey(key);

    public void RequestShutdown() => _pcControl.Shutdown();
    public void RequestRestart() => _pcControl.Restart();
    public void RequestSleep() => _pcControl.Sleep();
    public void RequestLogoff() => _pcControl.Logoff();

    private void OnInputReceived(object? sender, InputEvent e)
    {
        if (e.Action == InputAction.EmergencyStop)
        {
            NavigateTo("HomeViewModel");
            _scanEngine.Stop();
            return;
        }
        if (e.Action == InputAction.PauseToggle)
        {
            TogglePause();
        }
    }
}
