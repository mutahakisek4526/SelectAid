using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace SelectAid.Views;

public partial class ConfirmDialog : Window, INotifyPropertyChanged
{
    private readonly DispatcherTimer _timer;
    private int _elapsed;

    public string TitleText { get; }
    public string Message { get; }
    private int _holdProgress;
    public int HoldProgress
    {
        get => _holdProgress;
        private set
        {
            _holdProgress = value;
            RaisePropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public ConfirmDialog(string title, string message)
    {
        InitializeComponent();
        TitleText = title;
        Message = message;
        DataContext = this;
        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
        _timer.Tick += (_, _) => Tick();
    }

    private void ConfirmDown(object sender, MouseButtonEventArgs e)
    {
        _elapsed = 0;
        HoldProgress = 0;
        _timer.Start();
    }

    private void ConfirmUp(object sender, MouseButtonEventArgs e)
    {
        _timer.Stop();
        HoldProgress = 0;
    }

    private void Tick()
    {
        _elapsed += 50;
        HoldProgress = _elapsed;
        if (_elapsed >= 1500)
        {
            _timer.Stop();
            DialogResult = true;
        }
    }

    private void Cancel(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private void RaisePropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
