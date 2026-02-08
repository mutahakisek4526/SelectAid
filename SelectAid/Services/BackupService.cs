using System.IO.Compression;
using SelectAid.Persistence;

namespace SelectAid.Services;

public class BackupService
{
    private readonly LogService _log;

    public BackupService(LogService log)
    {
        _log = log;
    }

    public string CreateBackup()
    {
        AppPaths.Ensure();
        var stamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var path = Path.Combine(AppPaths.BackupsDir, $"backup_{stamp}.zip");
        using var archive = ZipFile.Open(path, ZipArchiveMode.Create);
        AddIfExists(archive, AppPaths.SettingsPath);
        AddIfExists(archive, AppPaths.ProfilesPath);
        AddIfExists(archive, AppPaths.KeyboardLayoutsPath);
        AddIfExists(archive, AppPaths.PhrasesPath);
        AddIfExists(archive, AppPaths.UserDictPath);
        AddIfExists(archive, AppPaths.HistoryPath);
        _log.Write("INFO", $"Backup created {path}");
        return path;
    }

    public void RestoreBackup(string zipPath)
    {
        if (!File.Exists(zipPath))
        {
            throw new FileNotFoundException("Backup not found", zipPath);
        }
        AppPaths.Ensure();
        ZipFile.ExtractToDirectory(zipPath, AppPaths.Root, true);
        _log.Write("INFO", $"Backup restored {zipPath}");
    }

    private static void AddIfExists(ZipArchive archive, string path)
    {
        if (File.Exists(path))
        {
            archive.CreateEntryFromFile(path, Path.GetFileName(path));
        }
    }
}
