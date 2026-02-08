using System.Windows.Input;
using SelectAid.Models;
using SelectAid.Services;

namespace SelectAid.Input;

public class InputRouter
{
    private readonly AppStateService _state;
    private readonly LogService _log;
    private bool _paused;

    public event EventHandler<InputEvent>? InputReceived;

    public InputRouter(AppStateService state, LogService log)
    {
        _state = state;
        _log = log;
    }

    public void HandleKey(Key key)
    {
        var profile = _state.CurrentProfile;
        var map = profile.Switch.KeyMap;
        if (IsKey(map.Stop, key))
        {
            Raise(InputAction.EmergencyStop);
            return;
        }

        if (IsKey(map.Lock, key))
        {
            Raise(InputAction.PauseToggle);
            return;
        }

        if (_paused)
        {
            return;
        }

        if (IsKey(map.Next, key))
        {
            Raise(InputAction.PointerMove, 0, 1);
            return;
        }

        if (IsKey(map.Select, key))
        {
            Raise(InputAction.Confirm);
            return;
        }

        if (IsKey(map.Back, key))
        {
            Raise(InputAction.Cancel);
        }
    }

    public void SetPaused(bool paused) => _paused = paused;

    private void Raise(InputAction action, double x = 0, double y = 0)
    {
        InputReceived?.Invoke(this, new InputEvent(action, x, y));
        _log.Write("INFO", $"Input {action}");
    }

    private static bool IsKey(string? keyName, Key key)
    {
        if (string.IsNullOrWhiteSpace(keyName))
        {
            return false;
        }
        if (Enum.TryParse<Key>(keyName, true, out var parsed))
        {
            return parsed == key;
        }
        return false;
    }
}
