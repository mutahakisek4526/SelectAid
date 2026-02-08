using System.Collections.ObjectModel;
using SelectAid.Models;
using SelectAid.Services;

namespace SelectAid.ViewModels;

public class PhrasesViewModel : ViewModelBase
{
    private readonly MainViewModel _main;
    private readonly AppStateService _state;
    private readonly SpeechService _speech;

    public ObservableCollection<PhraseScene> Scenes => _state.Phrases.Scenes;
    public PhraseScene? SelectedScene { get; set; }
    public PhraseCategory? SelectedCategory { get; set; }
    public PhraseItem? SelectedItem { get; set; }

    public RelayCommand AddSceneCommand { get; }
    public RelayCommand AddCategoryCommand { get; }
    public RelayCommand AddPhraseCommand { get; }
    public RelayCommand DeletePhraseCommand { get; }
    public RelayCommand SpeakPhraseCommand { get; }
    public RelayCommand CopyPhraseCommand { get; }
    public RelayCommand BackCommand { get; }

    public string NewSceneName { get; set; } = "新しい場面";
    public string NewCategoryName { get; set; } = "カテゴリ";
    public string NewPhraseText { get; set; } = "";

    public PhrasesViewModel(MainViewModel main, AppStateService state, SpeechService speech)
    {
        _main = main;
        _state = state;
        _speech = speech;
        AddSceneCommand = new RelayCommand(_ => AddScene());
        AddCategoryCommand = new RelayCommand(_ => AddCategory());
        AddPhraseCommand = new RelayCommand(_ => AddPhrase());
        DeletePhraseCommand = new RelayCommand(_ => DeletePhrase());
        SpeakPhraseCommand = new RelayCommand(_ => SpeakPhrase());
        CopyPhraseCommand = new RelayCommand(_ => CopyPhrase());
        BackCommand = new RelayCommand(_ => _main.NavigateTo(nameof(HomeViewModel)));
    }

    private void AddScene()
    {
        var scene = new PhraseScene { Name = NewSceneName };
        Scenes.Add(scene);
        _state.SaveAll();
    }

    private void AddCategory()
    {
        if (SelectedScene == null)
        {
            return;
        }
        var category = new PhraseCategory { Name = NewCategoryName };
        SelectedScene.Categories.Add(category);
        _state.SaveAll();
    }

    private void AddPhrase()
    {
        if (SelectedCategory == null)
        {
            return;
        }
        var item = new PhraseItem { Text = NewPhraseText };
        SelectedCategory.Items.Add(item);
        _state.SaveAll();
    }

    private void DeletePhrase()
    {
        if (SelectedCategory == null || SelectedItem == null)
        {
            return;
        }
        SelectedCategory.Items.Remove(SelectedItem);
        _state.SaveAll();
    }

    private void SpeakPhrase()
    {
        if (SelectedItem == null)
        {
            return;
        }
        _speech.Speak(SelectedItem.Text);
        _state.AppendHistory(SelectedItem.Text);
    }

    private void CopyPhrase()
    {
        if (SelectedItem == null)
        {
            return;
        }
        _main.NavigateTo(nameof(AacViewModel));
        if (_main.Navigation.CurrentView is AacViewModel aac)
        {
            aac.ComposeText += SelectedItem.Text;
        }
    }
}
