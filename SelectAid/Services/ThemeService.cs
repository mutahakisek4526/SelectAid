using System.Windows;

namespace SelectAid.Services;

public class ThemeService
{
    private const string BasePath = "Theme/Base.xaml";

    public void ApplyTheme(string themeId)
    {
        var app = Application.Current;
        if (app == null)
        {
            return;
        }

        var dictionaries = app.Resources.MergedDictionaries;
        dictionaries.Clear();
        dictionaries.Add(new ResourceDictionary { Source = new Uri(BasePath, UriKind.Relative) });
        dictionaries.Add(new ResourceDictionary
        {
            Source = new Uri($"Theme/{themeId}.xaml", UriKind.Relative)
        });
    }
}
