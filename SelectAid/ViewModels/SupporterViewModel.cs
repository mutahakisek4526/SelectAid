using System.Collections.ObjectModel;
using SelectAid.Models;
using SelectAid.Services;

namespace SelectAid.ViewModels;

public class SupporterViewModel : ViewModelBase
{
    private readonly MainViewModel _main;
    private readonly AppStateService _state;

    public ObservableCollection<Profile> Profiles => _state.Profiles.Profiles;
    public Profile? SelectedProfile { get; set; }

    public RelayCommand AddProfileCommand { get; }
    public RelayCommand SetActiveCommand { get; }
    public RelayCommand BackCommand { get; }

    public SupporterViewModel(MainViewModel main, AppStateService state)
    {
        _main = main;
        _state = state;
        SelectedProfile = Profiles.FirstOrDefault();
        AddProfileCommand = new RelayCommand(_ => AddProfile());
        SetActiveCommand = new RelayCommand(_ => SetActive());
        BackCommand = new RelayCommand(_ => _main.NavigateTo(nameof(HomeViewModel)));
    }

    private void AddProfile()
    {
        var profile = new Profile { Name = "Profile " + (Profiles.Count + 1) };
        Profiles.Add(profile);
        _state.SaveAll();
    }

    private void SetActive()
    {
        if (SelectedProfile == null)
        {
            return;
        }
        _state.Settings.CurrentProfileId = SelectedProfile.Id;
        _state.SaveAll();
        RaisePropertyChanged(nameof(SelectedProfile));
    }
}
