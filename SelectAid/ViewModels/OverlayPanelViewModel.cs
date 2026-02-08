using SelectAid.Models;
using SelectAid.Services;

namespace SelectAid.ViewModels;

public class OverlayPanelViewModel : ViewModelBase
{
    private readonly MainViewModel _main;
    private readonly AppStateService _state;

    public PcControlSettings PcControl => _state.CurrentProfile.PCControl;
    public RelayCommand ShowOverlayCommand { get; }
    public RelayCommand ShowMouseGridCommand { get; }
    public RelayCommand BackCommand { get; }

    public OverlayPanelViewModel(MainViewModel main, AppStateService state)
    {
        _main = main;
        _state = state;
        ShowOverlayCommand = new RelayCommand(_ => _main.ShowOverlay());
        ShowMouseGridCommand = new RelayCommand(_ => _main.ShowMouseGrid());
        BackCommand = new RelayCommand(_ => _main.NavigateTo(nameof(HomeViewModel)));
    }
}
