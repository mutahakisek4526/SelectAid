namespace SelectAid.ViewModels;

public class HomeViewModel : ViewModelBase
{
    private readonly MainViewModel _main;

    public RelayCommand GoAacCommand { get; }
    public RelayCommand GoPhrasesCommand { get; }
    public RelayCommand GoLayoutsCommand { get; }
    public RelayCommand GoOverlayCommand { get; }
    public RelayCommand GoSettingsCommand { get; }
    public RelayCommand GoSupporterCommand { get; }
    public RelayCommand GoTrainingCommand { get; }
    public RelayCommand GoLogsCommand { get; }
    public RelayCommand GoBackupCommand { get; }
    public RelayCommand ShutdownCommand { get; }
    public RelayCommand RestartCommand { get; }
    public RelayCommand SleepCommand { get; }
    public RelayCommand LogoffCommand { get; }

    public HomeViewModel(MainViewModel main)
    {
        _main = main;
        GoAacCommand = new RelayCommand(_ => _main.NavigateTo(nameof(AacViewModel)));
        GoPhrasesCommand = new RelayCommand(_ => _main.NavigateTo(nameof(PhrasesViewModel)));
        GoLayoutsCommand = new RelayCommand(_ => _main.NavigateTo(nameof(KeyboardLayoutsViewModel)));
        GoOverlayCommand = new RelayCommand(_ => _main.NavigateTo(nameof(OverlayPanelViewModel)));
        GoSettingsCommand = new RelayCommand(_ => _main.NavigateTo(nameof(SettingsViewModel)));
        GoSupporterCommand = new RelayCommand(_ => _main.NavigateTo(nameof(SupporterViewModel)));
        GoTrainingCommand = new RelayCommand(_ => _main.NavigateTo(nameof(TrainingViewModel)));
        GoLogsCommand = new RelayCommand(_ => _main.NavigateTo(nameof(LogsViewModel)));
        GoBackupCommand = new RelayCommand(_ => _main.NavigateTo(nameof(BackupRestoreViewModel)));
        ShutdownCommand = new RelayCommand(_ => _main.RequestShutdown());
        RestartCommand = new RelayCommand(_ => _main.RequestRestart());
        SleepCommand = new RelayCommand(_ => _main.RequestSleep());
        LogoffCommand = new RelayCommand(_ => _main.RequestLogoff());
    }
}
