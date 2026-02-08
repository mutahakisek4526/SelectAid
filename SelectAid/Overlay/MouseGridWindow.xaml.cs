using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SelectAid.Overlay;

public partial class MouseGridWindow : Window
{
    private readonly InputSender _sender = new();
    private readonly int _divisions;
    private Rect _region;
    private int _level;

    public MouseGridWindow(int divisions)
    {
        InitializeComponent();
        _divisions = Math.Clamp(divisions, 2, 6);
        Loaded += (_, _) => InitializeRegion();
        MouseLeftButtonDown += OnMouseDown;
        KeyDown += OnKeyDown;
    }

    private void InitializeRegion()
    {
        _region = new Rect(0, 0, SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);
        _level = 1;
        RenderGrid();
    }

    private void RenderGrid()
    {
        GridCanvas.Children.Clear();
        double cellWidth = _region.Width / _divisions;
        double cellHeight = _region.Height / _divisions;
        for (int i = 0; i <= _divisions; i++)
        {
            var v = new Line
            {
                X1 = _region.X + i * cellWidth,
                Y1 = _region.Y,
                X2 = _region.X + i * cellWidth,
                Y2 = _region.Y + _region.Height,
                Stroke = Brushes.Yellow,
                StrokeThickness = 2
            };
            var h = new Line
            {
                X1 = _region.X,
                Y1 = _region.Y + i * cellHeight,
                X2 = _region.X + _region.Width,
                Y2 = _region.Y + i * cellHeight,
                Stroke = Brushes.Yellow,
                StrokeThickness = 2
            };
            GridCanvas.Children.Add(v);
            GridCanvas.Children.Add(h);
        }
    }

    private void OnMouseDown(object? sender, MouseButtonEventArgs e)
    {
        var pos = e.GetPosition(this);
        var cellWidth = _region.Width / _divisions;
        var cellHeight = _region.Height / _divisions;
        var col = (int)((pos.X - _region.X) / cellWidth);
        var row = (int)((pos.Y - _region.Y) / cellHeight);
        col = Math.Clamp(col, 0, _divisions - 1);
        row = Math.Clamp(row, 0, _divisions - 1);
        _region = new Rect(_region.X + col * cellWidth, _region.Y + row * cellHeight, cellWidth, cellHeight);
        _level++;
        RenderGrid();
        if (_level >= 3)
        {
            SetCursorPos((int)(_region.X + _region.Width / 2), (int)(_region.Y + _region.Height / 2));
        }
    }

    private void LeftClick(object sender, RoutedEventArgs e)
    {
        _sender.LeftClick();
    }

    private void RightClick(object sender, RoutedEventArgs e)
    {
        _sender.RightClick();
    }

    private void Back(object sender, RoutedEventArgs e)
    {
        if (_level <= 1)
        {
            return;
        }
        _level--;
        var scale = Math.Pow(_divisions, 1);
        _region = new Rect(0, 0, SystemParameters.PrimaryScreenWidth / scale, SystemParameters.PrimaryScreenHeight / scale);
        RenderGrid();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            Back(sender, new RoutedEventArgs());
        }
    }

    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int x, int y);
}
