using SelectAid.Views;

namespace SelectAid.Services;

public class ConfirmService
{
    public bool Confirm(string title, string message)
    {
        var dialog = new ConfirmDialog(title, message);
        return dialog.ShowDialog() == true;
    }
}
