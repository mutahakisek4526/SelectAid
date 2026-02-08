namespace SelectAid.ViewModels;

public class TrainingViewModel : ViewModelBase
{
    private readonly MainViewModel _main;
    private int _attempts;
    private int _misses;

    public int Attempts
    {
        get => _attempts;
        private set
        {
            _attempts = value;
            RaisePropertyChanged();
        }
    }

    public int Misses
    {
        get => _misses;
        private set
        {
            _misses = value;
            RaisePropertyChanged();
        }
    }

    public RelayCommand HitCommand { get; }
    public RelayCommand MissCommand { get; }
    public RelayCommand BackCommand { get; }

    public TrainingViewModel(MainViewModel main)
    {
        _main = main;
        HitCommand = new RelayCommand(_ => Attempts++);
        MissCommand = new RelayCommand(_ => Misses++);
        BackCommand = new RelayCommand(_ => _main.NavigateTo(nameof(HomeViewModel)));
    }
}
