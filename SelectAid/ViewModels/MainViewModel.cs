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
    private readonly ConfirmService _confirm;
    private readonly LogService _log;
    private readonly ThemeService _themeService;

    public NavigationService Navigation { get; }
    public ViewModelBase CurrentView => Navigation.CurrentView;
    public Profile CurrentProfile => _state.CurrentProfile;
    public bool IsPaused { get; private set; }
    public bool IsCareLocked => _state.CurrentProfile.Safety.CareLockEnabled;
    public string Breadcrumb { get; private set; } = "Home";
    public string ScanStatus { get; private set; } = "L1";
    public string StatusMessage { get; private set; } = string.Empty;

    public RelayCommand HomeCommand { get; }
    public RelayCommand BackCommand { get; }
    public RelayCommand UndoCommand { get; }
    public RelayCommand PauseCommand { get; }
    public RelayCommand BuzzerCommand { get; }
    public RelayCommand OverlayCommand { get; }
    public RelayCommand ShutdownCommand { get; }
    public RelayCommand RestartCommand { get; }
    public RelayCommand SleepCommand { get; }
    public RelayCommand LogoffCommand { get; }

    public MainViewModel(AppStateService state, SpeechService speech, OverlayService overlay, InputRouter inputRouter,
        Scan.ScanEngine scanEngine, BuzzerService buzzer, StartupService startup, PcControlService pcControl,
        PredictionService prediction, BackupService backup, ConfirmService confirm, LogService log)
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
        _confirm = confirm;
        _log = log;
        _themeService = new ThemeService();

        var home = new HomeViewModel(this);
        var aac = new AacViewModel(this, _state, _speech, _prediction, _buzzer);
        var phrases = new PhrasesViewModel(this, _state, _speech);
        var layouts = new KeyboardLayoutsViewModel(this, _state);
        var overlayPanel = new OverlayPanelViewModel(this, _state);
        var settings = new SettingsViewModel(this, _state, _startup, _speech);
        var supporter = new SupporterViewModel(this, _state);
        var training = new TrainingViewModel(this);
        var logs = new LogsViewModel(this, _log);
        var backupRestore = new BackupRestoreViewModel(this, _backup, _state, _log, _confirm);

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
        Navigation.CurrentViewChanged += OnViewChanged;

        _overlay.HomeRequested += () => NavigateTo("HomeViewModel");
        _overlay.PauseRequested += TogglePause;
        _overlay.MouseGridRequested += ShowMouseGrid;
        _inputRouter.InputReceived += OnInputReceived;

        HomeCommand = new RelayCommand(_ => NavigateTo(nameof(HomeViewModel)));
        BackCommand = new RelayCommand(_ => Back());
        UndoCommand = new RelayCommand(_ => Undo());
        PauseCommand = new RelayCommand(_ => TogglePause());
        BuzzerCommand = new RelayCommand(_ => _buzzer.Buzz());
        OverlayCommand = new RelayCommand(_ => ShowOverlay());
        ShutdownCommand = new RelayCommand(_ => RequestShutdown());
        RestartCommand = new RelayCommand(_ => RequestRestart());
        SleepCommand = new RelayCommand(_ => RequestSleep());
        LogoffCommand = new RelayCommand(_ => RequestLogoff());

        ApplyTheme();
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

    public void RequestShutdown()
    {
        if (!_state.CurrentProfile.PCControl.AllowShutdown)
        {
            StatusMessage = "シャットダウンは支援者設定で無効です。";
            RaisePropertyChanged(nameof(StatusMessage));
            return;
        }
        if (_confirm.Confirm("電源", "シャットダウンしますか？"))
        {
            _pcControl.Shutdown();
        }
    }

    public void RequestRestart()
    {
        if (!_state.CurrentProfile.PCControl.AllowRestart)
        {
            StatusMessage = "再起動は支援者設定で無効です。";
            RaisePropertyChanged(nameof(StatusMessage));
            return;
        }
        if (_confirm.Confirm("電源", "再起動しますか？"))
        {
            _pcControl.Restart();
        }
    }

    public void RequestSleep()
    {
        if (!_state.CurrentProfile.PCControl.AllowSleep)
        {
            StatusMessage = "スリープは支援者設定で無効です。";
            RaisePropertyChanged(nameof(StatusMessage));
            return;
        }
        if (_confirm.Confirm("電源", "スリープしますか？"))
        {
            _pcControl.Sleep();
        }
    }

    public void RequestLogoff()
    {
        if (_confirm.Confirm("電源", "ログオフしますか？"))
        {
            _pcControl.Logoff();
        }
    }

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
        if (e.Action == InputAction.PointerMove && _state.CurrentProfile.InputMode == InputMode.SwitchScan)
        {
            _scanEngine.Step();
            UpdateScanStatus();
        }
        if (e.Action == InputAction.Confirm && _state.CurrentProfile.InputMode == InputMode.SwitchScan)
        {
            _scanEngine.Confirm();
        }
        if (e.Action == InputAction.Cancel && _state.CurrentProfile.InputMode == InputMode.SwitchScan)
        {
            _scanEngine.Back();
            UpdateScanStatus();
        }
    }

    private void OnViewChanged()
    {
        RaisePropertyChanged(nameof(CurrentView));
        Breadcrumb = $"Home > {Navigation.CurrentView.GetType().Name.Replace(\"ViewModel\", string.Empty)}";
        RaisePropertyChanged(nameof(Breadcrumb));
        UpdateScanStatus();
    }

    private void Back()
    {
        if (Navigation.CanGoBack)
        {
            Navigation.GoBack();
        }
    }

    private void Undo()
    {
        if (Navigation.CurrentView is IUndoable undoable)
        {
            undoable.Undo();
        }
    }

    private void UpdateScanStatus()
    {
        ScanStatus = $"L{_scanEngine.Level}";
        RaisePropertyChanged(nameof(ScanStatus));
    }

    private void ApplyTheme()
    {
        _themeService.ApplyTheme(_state.CurrentProfile.ThemeId);
    }
}
