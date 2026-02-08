using SelectAid.Models;
using SelectAid.Overlay;

namespace SelectAid.Services;

public class OverlayService
{
    private OverlayWindow? _overlay;

    public event Action? HomeRequested;
    public event Action? PauseRequested;
    public event Action? MouseGridRequested;

    public void Show(Profile profile)
    {
        if (!profile.PCControl.OverlayEnabled)
        {
            return;
        }
        if (_overlay != null)
        {
            _overlay.Activate();
            return;
        }
        _overlay = new OverlayWindow();
        _overlay.Closed += (_, _) => _overlay = null;
        _overlay.HomeRequested += () => HomeRequested?.Invoke();
        _overlay.PauseRequested += () => PauseRequested?.Invoke();
        _overlay.MouseGridRequested += () => MouseGridRequested?.Invoke();
        _overlay.Show();
    }

    public void Close()
    {
        _overlay?.Close();
        _overlay = null;
    }
}
