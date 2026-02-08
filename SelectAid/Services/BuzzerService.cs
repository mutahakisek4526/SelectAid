using System.Media;

namespace SelectAid.Services;

public class BuzzerService
{
    public void Buzz()
    {
        SystemSounds.Exclamation.Play();
        var window = System.Windows.Application.Current?.MainWindow;
        if (window != null)
        {
            var original = window.Background;
            window.Background = System.Windows.Media.Brushes.Yellow;
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(200)
            };
            timer.Tick += (_, _) =>
            {
                timer.Stop();
                window.Background = original;
            };
            timer.Start();
        }
    }
}
