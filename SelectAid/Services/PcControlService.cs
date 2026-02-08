using System.Diagnostics;

namespace SelectAid.Services;

public class PcControlService
{
    public void Shutdown() => Run("/s /t 0");
    public void Restart() => Run("/r /t 0");
    public void Sleep() => Run("/h");
    public void Logoff() => Run("/l");

    private static void Run(string args)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "shutdown.exe",
            Arguments = args,
            CreateNoWindow = true,
            UseShellExecute = false
        });
    }
}
