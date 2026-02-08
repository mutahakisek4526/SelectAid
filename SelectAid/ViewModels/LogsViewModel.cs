using System.IO;
using SelectAid.Persistence;
using SelectAid.Services;

namespace SelectAid.ViewModels;

public class LogsViewModel : ViewModelBase
{
    private readonly MainViewModel _main;
    private readonly LogService _log;

    public string LogText { get; private set; } = string.Empty;

    public RelayCommand RefreshCommand { get; }
    public RelayCommand ExportCsvCommand { get; }
    public RelayCommand BackCommand { get; }

    public LogsViewModel(MainViewModel main, LogService log)
    {
        _main = main;
        _log = log;
        RefreshCommand = new RelayCommand(_ => Load());
        ExportCsvCommand = new RelayCommand(_ => ExportCsv());
        BackCommand = new RelayCommand(_ => _main.NavigateTo(nameof(HomeViewModel)));
        Load();
    }

    private void Load()
    {
        if (File.Exists(AppPaths.LogPath))
        {
            LogText = File.ReadAllText(AppPaths.LogPath);
            RaisePropertyChanged(nameof(LogText));
        }
    }

    private void ExportCsv()
    {
        var output = Path.Combine(AppPaths.Root, "usage.csv");
        var lines = new List<string> { "timestamp,level,message" };
        if (File.Exists(AppPaths.LogPath))
        {
            foreach (var line in File.ReadAllLines(AppPaths.LogPath))
            {
                lines.Add(line.Replace(" ", ","));
            }
        }
        File.WriteAllLines(output, lines);
        _log.Write("INFO", $"CSV exported {output}");
    }
}
