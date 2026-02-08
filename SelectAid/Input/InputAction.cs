namespace SelectAid.Input;

public enum InputAction
{
    PointerMove,
    PointerMoveAbs,
    LeftClick,
    RightClick,
    DoubleClick,
    DragStart,
    DragEnd,
    ScrollUp,
    ScrollDown,
    Confirm,
    Cancel,
    EmergencyStop,
    PauseToggle
}

public record InputEvent(InputAction Action, double X = 0, double Y = 0);
