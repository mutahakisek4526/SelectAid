using System.Windows.Threading;
using SelectAid.Models;
using SelectAid.Services;

namespace SelectAid.Scan;

public class ScanEngine
{
    private readonly DispatcherTimer _timer;
    private readonly AppStateService _state;
    private readonly LogService _log;
    private int _index;
    private int _level = 1;
    private int _cycles;
    private bool _paused;

    public event Action<int, int>? HighlightChanged;
    public event Action<int, int>? Confirmed;
    public event Action? Stopped;

    public int Level => _level;

    public ScanEngine(AppStateService state, LogService log)
    {
        _state = state;
        _log = log;
        _timer = new DispatcherTimer();
        _timer.Tick += (_, _) => Step();
    }

    public void Start()
    {
        _cycles = 0;
        _level = 1;
        _index = -1;
        _paused = false;
        Schedule();
        _timer.Start();
        _log.Write("INFO", "Scan started");
    }

    public void Stop()
    {
        _timer.Stop();
        _paused = true;
        Stopped?.Invoke();
        _log.Write("INFO", "Scan stopped");
    }

    public void PauseToggle()
    {
        _paused = !_paused;
        _log.Write("INFO", $"Scan pause {_paused}");
    }

    public void Confirm()
    {
        Confirmed?.Invoke(_level, _index);
    }

    public void Back()
    {
        if (_level > 1)
        {
            _level--;
            _index = -1;
            Schedule();
        }
    }

    public void SetLevel(int level)
    {
        _level = level;
        _index = -1;
        Schedule();
    }

    public void Step(int? maxItems = null)
    {
        if (_paused)
        {
            return;
        }
        var max = maxItems ?? 10;
        if (max <= 0)
        {
            return;
        }
        _index = (_index + 1) % max;
        if (_index == 0)
        {
            _cycles++;
            if (_cycles >= _state.CurrentProfile.Scan.AutoStopAfterCycles)
            {
                Stop();
                return;
            }
        }
        HighlightChanged?.Invoke(_level, _index);
    }

    private void Schedule()
    {
        var scan = _state.CurrentProfile.Scan;
        _timer.Interval = TimeSpan.FromMilliseconds(_level switch
        {
            1 => scan.Level1SpeedMs,
            2 => scan.Level2SpeedMs,
            _ => scan.Level3SpeedMs
        });
    }
}
