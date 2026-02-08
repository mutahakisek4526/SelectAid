using System.Collections.ObjectModel;
using SelectAid.Models;
using SelectAid.Services;

namespace SelectAid.ViewModels;

public class AacViewModel : ViewModelBase, IUndoable
{
    private readonly MainViewModel _main;
    private readonly AppStateService _state;
    private readonly SpeechService _speech;
    private readonly PredictionService _prediction;
    private readonly BuzzerService _buzzer;

    private string _composeText = string.Empty;
    private KeyboardLayout? _selectedLayout;
    private readonly Stack<string> _undoStack = new();

    public ObservableCollection<HistoryItem> History => _state.History.Items;
    public ObservableCollection<KeyboardLayout> Layouts => _state.KeyboardLayouts.Layouts;
    public ObservableCollection<string> Candidates { get; } = new();

    public KeyboardLayout? SelectedLayout
    {
        get => _selectedLayout;
        set
        {
            _selectedLayout = value;
            RaisePropertyChanged();
        }
    }

    public string ComposeText
    {
        get => _composeText;
        set
        {
            _composeText = value;
            RaisePropertyChanged();
            UpdateCandidates();
        }
    }

    public RelayCommand SpeakCommand { get; }
    public RelayCommand ClearCommand { get; }
    public RelayCommand BackspaceCommand { get; }
    public RelayCommand UndoCommand { get; }
    public RelayCommand BuzzerCommand { get; }
    public RelayCommand PauseCommand { get; }
    public RelayCommand OverlayCommand { get; }
    public RelayCommand GoHomeCommand { get; }
    public RelayCommand GoPhrasesCommand { get; }
    public RelayCommand ApplyKeyCommand { get; }
    public RelayCommand ApplyCandidateCommand { get; }

    public AacViewModel(MainViewModel main, AppStateService state, SpeechService speech, PredictionService prediction, BuzzerService buzzer)
    {
        _main = main;
        _state = state;
        _speech = speech;
        _prediction = prediction;
        _buzzer = buzzer;
        _selectedLayout = Layouts.FirstOrDefault(l => l.Enabled);

        SpeakCommand = new RelayCommand(_ => Speak());
        ClearCommand = new RelayCommand(_ => Clear());
        BackspaceCommand = new RelayCommand(_ => Backspace());
        UndoCommand = new RelayCommand(_ => Undo());
        BuzzerCommand = new RelayCommand(_ => _buzzer.Buzz());
        PauseCommand = new RelayCommand(_ => _main.TogglePause());
        OverlayCommand = new RelayCommand(_ => _main.ShowOverlay());
        GoHomeCommand = new RelayCommand(_ => _main.NavigateTo(nameof(HomeViewModel)));
        GoPhrasesCommand = new RelayCommand(_ => _main.NavigateTo(nameof(PhrasesViewModel)));
        ApplyKeyCommand = new RelayCommand(param =>
        {
            if (param is KeyDefinition key)
            {
                ApplyKey(key);
            }
        });
        ApplyCandidateCommand = new RelayCommand(param =>
        {
            if (param is string candidate)
            {
                ApplyCandidate(candidate);
            }
        });
    }

    public void ApplyKey(KeyDefinition key)
    {
        if (!string.IsNullOrWhiteSpace(key.Action))
        {
            HandleAction(key.Action);
            return;
        }
        PushUndo();
        ComposeText += key.OutputText;
    }

    public void ApplyCandidate(string candidate)
    {
        PushUndo();
        ComposeText = candidate + " ";
    }

    private void HandleAction(string action)
    {
        switch (action)
        {
            case "Backspace":
                Backspace();
                break;
            case "Clear":
                Clear();
                break;
            case "Speak":
                Speak();
                break;
            case "Undo":
                Undo();
                break;
            case "GoPhrases":
                _main.NavigateTo(nameof(PhrasesViewModel));
                break;
            case "GoHome":
                _main.NavigateTo(nameof(HomeViewModel));
                break;
            case "ToggleOverlay":
                _main.ShowOverlay();
                break;
            case "Buzzer":
                _buzzer.Buzz();
                break;
            default:
                break;
        }
    }

    private void Speak()
    {
        if (string.IsNullOrWhiteSpace(ComposeText))
        {
            return;
        }
        _speech.Speak(ComposeText);
        _state.AppendHistory(ComposeText);
        if (_state.CurrentProfile.Speech.AutoClearAfterSpeak)
        {
            ComposeText = string.Empty;
        }
    }

    private void Clear()
    {
        PushUndo();
        ComposeText = string.Empty;
    }

    private void Backspace()
    {
        if (string.IsNullOrEmpty(ComposeText))
        {
            return;
        }
        PushUndo();
        ComposeText = ComposeText[..^1];
    }

    public void Undo()
    {
        if (_undoStack.TryPop(out var prev))
        {
            ComposeText = prev;
        }
    }

    private void PushUndo()
    {
        _undoStack.Push(ComposeText);
        if (_undoStack.Count > 10)
        {
            _undoStack.TrimExcess();
        }
    }

    private void UpdateCandidates()
    {
        Candidates.Clear();
        foreach (var item in _prediction.GetCandidates(ComposeText, _state.History, _state.UserDictionary))
        {
            Candidates.Add(item);
        }
    }
}
