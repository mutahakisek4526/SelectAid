using System.Collections.ObjectModel;

namespace SelectAid.Models;

public class KeyboardLayoutsStore
{
    public ObservableCollection<KeyboardLayout> Layouts { get; set; } = new();
}

public class KeyboardLayout
{
    public string Name { get; set; } = "Layout";
    public string Type { get; set; } = "Custom";
    public ObservableCollection<KeyboardRow> Grid { get; set; } = new();
    public ObservableCollection<ScanGroup> ScanGroups { get; set; } = new();
    public bool Enabled { get; set; } = true;
}

public class KeyboardRow
{
    public ObservableCollection<KeyDefinition> Cells { get; set; } = new();
}

public class KeyDefinition
{
    public string Label { get; set; } = string.Empty;
    public string OutputText { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Variant { get; set; } = string.Empty;
}

public class ScanGroup
{
    public string Name { get; set; } = string.Empty;
    public ObservableCollection<int> ItemIndices { get; set; } = new();
}

public class PhrasesStore
{
    public ObservableCollection<PhraseScene> Scenes { get; set; } = new();
}

public class PhraseScene
{
    public string Name { get; set; } = string.Empty;
    public ObservableCollection<PhraseCategory> Categories { get; set; } = new();
}

public class PhraseCategory
{
    public string Name { get; set; } = string.Empty;
    public ObservableCollection<PhraseItem> Items { get; set; } = new();
}

public class PhraseItem
{
    public string PhraseId { get; set; } = Guid.NewGuid().ToString("N");
    public string Text { get; set; } = string.Empty;
    public ObservableCollection<string> Tags { get; set; } = new();
}

public class UserDictionary
{
    public ObservableCollection<string> Words { get; set; } = new();
}

public class HistoryStore
{
    public ObservableCollection<HistoryItem> Items { get; set; } = new();
}

public class HistoryItem
{
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public string Text { get; set; } = string.Empty;
}
