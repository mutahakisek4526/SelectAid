using System.IO;
using System.Text;
using SelectAid.Persistence;

namespace SelectAid.Services;

public class LogService
{
    private readonly object _lock = new();

    public void Write(string level, string message, Exception? ex = null)
    {
        AppPaths.Ensure();
        var sb = new StringBuilder();
        sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        sb.Append(" [").Append(level).Append("] ");
        sb.Append(message);
        if (ex != null)
        {
            sb.AppendLine();
            sb.Append(ex);
        }
        sb.AppendLine();
        lock (_lock)
        {
            File.AppendAllText(AppPaths.LogPath, sb.ToString());
        }
    }
}
