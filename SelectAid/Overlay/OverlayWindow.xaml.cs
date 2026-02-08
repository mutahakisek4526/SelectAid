using System.Windows;
using System.Windows.Input;

namespace SelectAid.Overlay;

public partial class OverlayWindow : Window
{
    private readonly InputSender _sender = new();
    private bool _transparentMode;

    public event Action? HomeRequested;
    public event Action? PauseRequested;
    public event Action? MouseGridRequested;

    public OverlayWindow()
    {
        InitializeComponent();
    }

    private void LeftClick(object sender, RoutedEventArgs e) => _sender.LeftClick();
    private void RightClick(object sender, RoutedEventArgs e) => _sender.RightClick();
    private void DoubleClick(object sender, RoutedEventArgs e) => _sender.DoubleClick();
    private void DragStart(object sender, RoutedEventArgs e) => _sender.DragStart();
    private void DragEnd(object sender, RoutedEventArgs e) => _sender.DragEnd();
    private void ScrollUp(object sender, RoutedEventArgs e) => _sender.Scroll(120);
    private void ScrollDown(object sender, RoutedEventArgs e) => _sender.Scroll(-120);
    private void Back(object sender, RoutedEventArgs e) => _sender.SendKey((ushort)KeyInterop.VirtualKeyFromKey(Key.BrowserBack));
    private void Tab(object sender, RoutedEventArgs e) => _sender.SendKey((ushort)KeyInterop.VirtualKeyFromKey(Key.Tab));
    private void ShiftTab(object sender, RoutedEventArgs e)
    {
        _sender.SendKey((ushort)KeyInterop.VirtualKeyFromKey(Key.LeftShift));
        _sender.SendKey((ushort)KeyInterop.VirtualKeyFromKey(Key.Tab));
    }
    private void Enter(object sender, RoutedEventArgs e) => _sender.SendKey((ushort)KeyInterop.VirtualKeyFromKey(Key.Enter));
    private void Space(object sender, RoutedEventArgs e) => _sender.SendKey((ushort)KeyInterop.VirtualKeyFromKey(Key.Space));
    private void Home(object sender, RoutedEventArgs e) => HomeRequested?.Invoke();
    private void Pause(object sender, RoutedEventArgs e) => PauseRequested?.Invoke();
    private void MouseGrid(object sender, RoutedEventArgs e) => MouseGridRequested?.Invoke();

    private void ToggleTransparent(object sender, RoutedEventArgs e)
    {
        _transparentMode = !_transparentMode;
        IsHitTestVisible = !_transparentMode;
        Opacity = _transparentMode ? 0.4 : 1.0;
    }

    private void CloseOverlay(object sender, RoutedEventArgs e) => Close();
}
