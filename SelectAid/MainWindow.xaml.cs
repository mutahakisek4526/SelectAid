using System.Windows;
using System.Windows.Input;
using SelectAid.ViewModels;

namespace SelectAid;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        PreviewKeyDown += OnPreviewKeyDown;
    }

    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            vm.HandleKey(e.Key);
        }
    }
}
