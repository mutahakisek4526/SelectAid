using SelectAid.Services;

namespace SelectAid.ViewModels;

public class BackupRestoreViewModel : ViewModelBase
{
    private readonly MainViewModel _main;
    private readonly BackupService _backup;
    private readonly AppStateService _state;
    private readonly LogService _log;

    public string LastBackupPath { get; private set; } = string.Empty;
    public string RestorePath { get; set; } = string.Empty;

    public RelayCommand BackupCommand { get; }
    public RelayCommand RestoreCommand { get; }
    public RelayCommand BackCommand { get; }

    public BackupRestoreViewModel(MainViewModel main, BackupService backup, AppStateService state, LogService log)
    {
        _main = main;
        _backup = backup;
        _state = state;
        _log = log;
        BackupCommand = new RelayCommand(_ => Backup());
        RestoreCommand = new RelayCommand(_ => Restore());
        BackCommand = new RelayCommand(_ => _main.NavigateTo(nameof(HomeViewModel)));
    }

    private void Backup()
    {
        LastBackupPath = _backup.CreateBackup();
        RaisePropertyChanged(nameof(LastBackupPath));
    }

    private void Restore()
    {
        if (!_state.CurrentProfile.Safety.CareLockEnabled)
        {
            _backup.RestoreBackup(RestorePath);
            _state.Load();
        }
        else
        {
            _log.Write("WARN", "CareLock enabled. Restore blocked.");
        }
    }
}
