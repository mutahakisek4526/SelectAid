using System.Collections.ObjectModel;
using SelectAid.Models;
using SelectAid.Services;

namespace SelectAid.ViewModels;

public class KeyboardLayoutsViewModel : ViewModelBase
{
    private readonly MainViewModel _main;
    private readonly AppStateService _state;

    public ObservableCollection<KeyboardLayout> Layouts => _state.KeyboardLayouts.Layouts;
    public KeyboardLayout? SelectedLayout { get; set; }

    public RelayCommand EnableLayoutCommand { get; }
    public RelayCommand AddLayoutCommand { get; }
    public RelayCommand DeleteLayoutCommand { get; }
    public RelayCommand BackCommand { get; }

    public KeyboardLayoutsViewModel(MainViewModel main, AppStateService state)
    {
        _main = main;
        _state = state;
        EnableLayoutCommand = new RelayCommand(_ => EnableLayout());
        AddLayoutCommand = new RelayCommand(_ => AddLayout());
        DeleteLayoutCommand = new RelayCommand(_ => DeleteLayout());
        BackCommand = new RelayCommand(_ => _main.NavigateTo(nameof(HomeViewModel)));
    }

    private void EnableLayout()
    {
        if (SelectedLayout == null)
        {
            return;
        }
        SelectedLayout.Enabled = !SelectedLayout.Enabled;
        _state.SaveAll();
    }

    private void AddLayout()
    {
        var layout = new KeyboardLayout { Name = "新しいレイアウト", Type = "Custom" };
        layout.Grid.Add(new KeyboardRow { Cells = new() { new KeyDefinition { Label = "A", OutputText = "A" } } });
        Layouts.Add(layout);
        _state.SaveAll();
    }

    private void DeleteLayout()
    {
        if (SelectedLayout == null)
        {
            return;
        }
        Layouts.Remove(SelectedLayout);
        _state.SaveAll();
    }
}
