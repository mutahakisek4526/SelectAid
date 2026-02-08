using SelectAid.Models;

namespace SelectAid.Services;

public static class DefaultContent
{
    public static KeyboardLayoutsStore CreateDefaultLayouts()
    {
        var store = new KeyboardLayoutsStore();
        store.Layouts.Add(new KeyboardLayout
        {
            Name = "標準50音",
            Type = "Kana50",
            Grid = new()
            {
                new KeyboardRow { Cells = new() { Key("あ"), Key("い"), Key("う"), Key("え"), Key("お") } },
                new KeyboardRow { Cells = new() { Key("か"), Key("き"), Key("く"), Key("け"), Key("こ") } },
                new KeyboardRow { Cells = new() { Key("さ"), Key("し"), Key("す"), Key("せ"), Key("そ") } },
                new KeyboardRow { Cells = new() { Key("た"), Key("ち"), Key("つ"), Key("て"), Key("と") } },
                new KeyboardRow { Cells = new() { Key("な"), Key("に"), Key("ぬ"), Key("ね"), Key("の") } },
                new KeyboardRow { Cells = new() { Key("は"), Key("ひ"), Key("ふ"), Key("へ"), Key("ほ") } },
                new KeyboardRow { Cells = new() { Key("ま"), Key("み"), Key("む"), Key("め"), Key("も") } },
                new KeyboardRow { Cells = new() { Key("や"), Key("ゆ"), Key("よ"), Key("ー") } },
                new KeyboardRow { Cells = new() { Key("ら"), Key("り"), Key("る"), Key("れ"), Key("ろ") } },
                new KeyboardRow { Cells = new() { Key("わ"), Key("を"), Key("ん"), Action("⌫", "Backspace"), Action("話す", "Speak") } }
            }
        });
        store.Layouts.Add(new KeyboardLayout
        {
            Name = "視線向け配置",
            Type = "Custom",
            Grid = new()
            {
                new KeyboardRow { Cells = new() { Key("あ"), Key("い"), Key("う"), Key("え"), Key("お"), Action("⌫", "Backspace") } },
                new KeyboardRow { Cells = new() { Key("か"), Key("さ"), Key("た"), Key("な"), Key("は"), Key("ま") } },
                new KeyboardRow { Cells = new() { Key("や"), Key("ら"), Key("わ"), Key("ん"), Action("空白", " "), Action("話す", "Speak") } }
            }
        });
        store.Layouts.Add(new KeyboardLayout
        {
            Name = "TC行列",
            Type = "ScanRowCol",
            Grid = new()
            {
                new KeyboardRow { Cells = new() { Key("あ"), Key("い"), Key("う"), Key("え"), Key("お") } },
                new KeyboardRow { Cells = new() { Key("か"), Key("き"), Key("く"), Key("け"), Key("こ") } },
                new KeyboardRow { Cells = new() { Key("さ"), Key("し"), Key("す"), Key("せ"), Key("そ") } },
                new KeyboardRow { Cells = new() { Key("た"), Key("ち"), Key("つ"), Key("て"), Key("と") } }
            }
        });
        store.Layouts.Add(new KeyboardLayout
        {
            Name = "絵文字",
            Type = "Emoji",
            Grid = new()
            {
                new KeyboardRow { Cells = new() { Key("😊"), Key("😢"), Key("👍"), Key("🙏"), Key("❤️") } },
                new KeyboardRow { Cells = new() { Key("😠"), Key("🎉"), Key("🍀"), Key("☕"), Key("🏠") } }
            }
        });
        store.Layouts.Add(new KeyboardLayout
        {
            Name = "数字/記号",
            Type = "Symbols",
            Grid = new()
            {
                new KeyboardRow { Cells = new() { Key("1"), Key("2"), Key("3"), Key("4"), Key("5") } },
                new KeyboardRow { Cells = new() { Key("6"), Key("7"), Key("8"), Key("9"), Key("0") } },
                new KeyboardRow { Cells = new() { Key("?"), Key("!"), Key("/"), Key("-"), Key(".") } }
            }
        });
        store.Layouts.Add(new KeyboardLayout
        {
            Name = "カスタム",
            Type = "Custom",
            Grid = new()
            {
                new KeyboardRow { Cells = new() { Key(" ") } }
            }
        });
        return store;
    }

    public static PhrasesStore CreateDefaultPhrases()
    {
        var store = new PhrasesStore();
        var scene = new PhraseScene { Name = "生活" };
        var cat = new PhraseCategory { Name = "お願い" };
        cat.Items.Add(new PhraseItem { Text = "水をください" });
        cat.Items.Add(new PhraseItem { Text = "トイレに行きたい" });
        scene.Categories.Add(cat);
        store.Scenes.Add(scene);
        return store;
    }

    private static KeyDefinition Key(string label) => new() { Label = label, OutputText = label };
    private static KeyDefinition Action(string label, string action) => new() { Label = label, Action = action };
}
