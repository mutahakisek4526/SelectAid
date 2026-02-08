using System.Windows;
using System.Windows.Input;
using SelectAid.Services;
using SelectAid.Persistence;
using SelectAid.ViewModels;

namespace SelectAid;

public partial class App : Application
{
    private LogService? _log;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        AppPaths.Ensure();
        _log = new LogService();
        DispatcherUnhandledException += (_, args) =>
        {
            _log.Write("ERROR", "UI exception", args.Exception);
            args.Handled = true;
        };
        TaskScheduler.UnobservedTaskException += (_, args) =>
        {
            _log.Write("ERROR", "Task exception", args.Exception);
            args.SetObserved();
        };

        var jsonStore = new JsonStore();
        var state = new AppStateService(jsonStore, _log);
        state.Load();
        var speech = new SpeechService();
        speech.Configure(state.CurrentProfile.Speech);
        var overlay = new OverlayService();
        var inputRouter = new Input.InputRouter(state, _log);
        var scanEngine = new Scan.ScanEngine(state, _log);
        var buzzer = new BuzzerService();
        var startup = new StartupService();
        var pcControl = new PcControlService();
        var prediction = new PredictionService();
        var backup = new BackupService(_log);
        var confirm = new ConfirmService();

        var mainVm = new MainViewModel(state, speech, overlay, inputRouter, scanEngine, buzzer, startup, pcControl, prediction, backup, confirm, _log);
        if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
        {
            mainVm.NavigateTo(nameof(SupporterViewModel));
        }
        var window = new MainWindow { DataContext = mainVm };
        MainWindow = window;
        window.Show();
    }
}
