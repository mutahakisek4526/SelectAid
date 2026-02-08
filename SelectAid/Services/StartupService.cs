using Microsoft.Win32;

namespace SelectAid.Services;

public class StartupService
{
    private const string KeyPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
    private const string AppName = "SelectAid";

    public bool IsEnabled()
    {
        using var key = Registry.CurrentUser.OpenSubKey(KeyPath, false);
        return key?.GetValue(AppName) != null;
    }

    public void SetEnabled(bool enabled, string exePath)
    {
        using var key = Registry.CurrentUser.OpenSubKey(KeyPath, true) ??
                        Registry.CurrentUser.CreateSubKey(KeyPath);
        if (enabled)
        {
            key.SetValue(AppName, exePath);
        }
        else
        {
            key.DeleteValue(AppName, false);
        }
    }
}
