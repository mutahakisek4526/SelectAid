namespace SelectAid.ViewModels;

public interface IUndoable
{
    void Undo();
}

public interface IBackNavigable
{
    void Back();
}
