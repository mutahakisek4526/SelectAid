using SelectAid.ViewModels;

namespace SelectAid.Services;

public class NavigationService
{
    private readonly Dictionary<string, ViewModelBase> _views = new();

    public ViewModelBase CurrentView { get; private set; }
    public event Action? CurrentViewChanged;

    public NavigationService(ViewModelBase initialView)
    {
        CurrentView = initialView;
        _views[initialView.GetType().Name] = initialView;
    }

    public void Register(ViewModelBase viewModel)
    {
        _views[viewModel.GetType().Name] = viewModel;
    }

    public void NavigateTo(string viewModelName)
    {
        if (_views.TryGetValue(viewModelName, out var vm))
        {
            CurrentView = vm;
            CurrentViewChanged?.Invoke();
        }
    }
}
