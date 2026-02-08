using System.Collections.ObjectModel;

namespace SelectAid.Models;

public enum InputMode
{
    EyeOnly,
    EyeSwitch,
    EyeGyro,
    GyroOnly,
    SwitchScan
}

public enum DwellFeedback
{
    Ring,
    Fill,
    None
}

public enum AccelCurve
{
    Linear,
    Expo
}

public enum DragMode
{
    HoldToDrag,
    ToggleDrag
}

public class EyePointerSettings
{
    public double Sensitivity { get; set; } = 1.0;
    public double Smoothing { get; set; } = 0.4;
    public double JitterThreshold { get; set; } = 2;
    public double StopThreshold { get; set; } = 1;
    public double EdgeSnapStrength { get; set; } = 0.3;
}

public class DwellSettings
{
    public bool Enabled { get; set; } = true;
    public int PreDelayMs { get; set; } = 250;
    public int DwellMs { get; set; } = 900;
    public int AllowSmallDriftPx { get; set; } = 6;
    public int ResetOnLeaveMs { get; set; } = 120;
    public DwellFeedback VisualFeedback { get; set; } = DwellFeedback.Ring;
    public int ConfirmGuardMs { get; set; } = 300;
}

public class GyroSettings
{
    public double XGain { get; set; } = 1.0;
    public double YGain { get; set; } = 1.0;
    public double DeadZone { get; set; } = 0.05;
    public double MaxSpeed { get; set; } = 14;
    public AccelCurve AccelCurve { get; set; } = AccelCurve.Linear;
    public double CalibrationOffset { get; set; } = 0;
    public int ClickDebounceMs { get; set; } = 180;
    public DragMode DragMode { get; set; } = DragMode.HoldToDrag;
}

public class SwitchSettings
{
    public int DebounceMs { get; set; } = 80;
    public int PressMinMs { get; set; } = 60;
    public int LongPressMs { get; set; } = 500;
    public int RepeatBlockMs { get; set; } = 400;
    public int CancelGraceMs { get; set; } = 350;
    public SwitchKeyMap KeyMap { get; set; } = new();
}

public class SwitchKeyMap
{
    public string Next { get; set; } = "Space";
    public string Select { get; set; } = "Enter";
    public string Back { get; set; } = "Escape";
    public string Stop { get; set; } = "F12";
    public string Lock { get; set; } = "F11";
}

public class ScanSettings
{
    public int Level1SpeedMs { get; set; } = 900;
    public int Level2SpeedMs { get; set; } = 800;
    public int Level3SpeedMs { get; set; } = 750;
    public bool HoldToPauseEnabled { get; set; } = true;
    public int AutoStopAfterCycles { get; set; } = 4;
    public bool PriorityLastArea { get; set; } = true;
    public int ConfirmGuardMs { get; set; } = 300;
    public int UndoWindowMs { get; set; } = 1500;
}

public class UiSettings
{
    public string Scale { get; set; } = "Large";
    public bool HighContrast { get; set; }
    public double ButtonSpacing { get; set; } = 6;
    public double FontSizeBase { get; set; } = 20;
    public int MaxButtonsPerScreen { get; set; } = 16;
}

public class SpeechSettings
{
    public string VoiceId { get; set; } = string.Empty;
    public int Rate { get; set; } = 0;
    public int Volume { get; set; } = 100;
    public bool AutoClearAfterSpeak { get; set; } = false;
}

public class SafetySettings
{
    public bool CareLockEnabled { get; set; }
    public int IdleAutoLockMinutes { get; set; } = 0;
    public string EmergencyStopBinding { get; set; } = "F12";
}

public class PcControlSettings
{
    public bool OverlayEnabled { get; set; } = true;
    public bool MouseGridEnabled { get; set; } = true;
    public int MouseGridDivisions { get; set; } = 3;
    public bool OverlayScanEnabled { get; set; } = true;
    public bool AllowShutdown { get; set; } = false;
    public bool AllowSleep { get; set; } = false;
    public bool AllowRestart { get; set; } = false;
}

public class Profile
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Name { get; set; } = "Default";
    public InputMode InputMode { get; set; } = InputMode.EyeOnly;
    public EyePointerSettings EyePointer { get; set; } = new();
    public DwellSettings Dwell { get; set; } = new();
    public GyroSettings Gyro { get; set; } = new();
    public SwitchSettings Switch { get; set; } = new();
    public ScanSettings Scan { get; set; } = new();
    public UiSettings UI { get; set; } = new();
    public SpeechSettings Speech { get; set; } = new();
    public SafetySettings Safety { get; set; } = new();
    public PcControlSettings PCControl { get; set; } = new();
    public string ThemeId { get; set; } = "Friendly";
}

public class AppSettings
{
    public string CurrentProfileId { get; set; } = string.Empty;
    public bool StartFullscreen { get; set; } = false;
    public bool AutoStartEnabled { get; set; } = false;
    public int SchemaVersion { get; set; } = 1;
}

public class ProfilesStore
{
    public ObservableCollection<Profile> Profiles { get; set; } = new();
}
