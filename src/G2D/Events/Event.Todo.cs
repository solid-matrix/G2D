using SDL;

namespace G2D;

public readonly struct QuitEvent
{
    private readonly SDL_Event _e;

    internal QuitEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct TerminatingEvent
{
    private readonly SDL_Event _e;

    internal TerminatingEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct LowMemoryEvent
{
    private readonly SDL_Event _e;

    internal LowMemoryEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct WillEnterBackgroundEvent
{
    private readonly SDL_Event _e;

    internal WillEnterBackgroundEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct DidEnterForegroundEvent
{
    private readonly SDL_Event _e;

    internal DidEnterForegroundEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct LocaleChangedEvent
{
    private readonly SDL_Event _e;

    internal LocaleChangedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct SystemThemeChangedEvent
{
    private readonly SDL_Event _e;

    internal SystemThemeChangedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct DisplayOrientationEvent
{
    private readonly SDL_Event _e;

    internal DisplayOrientationEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct DisplayAddedEvent
{
    private readonly SDL_Event _e;

    internal DisplayAddedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct DisplayRemovedEvent
{
    private readonly SDL_Event _e;

    internal DisplayRemovedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct DisplayMovedEvent
{
    private readonly SDL_Event _e;

    internal DisplayMovedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct DisplayDesktopModeChangedEvent
{
    private readonly SDL_Event _e;

    internal DisplayDesktopModeChangedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct DisplayCurrentModeChangedEvent
{
    private readonly SDL_Event _e;

    internal DisplayCurrentModeChangedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct DisplayContentScaleChangedEvent
{
    private readonly SDL_Event _e;

    internal DisplayContentScaleChangedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct DisplayUsableBoundsChangedEvent
{
    private readonly SDL_Event _e;

    internal DisplayUsableBoundsChangedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct WindowExposedEvent
{
    private readonly SDL_Event _e;

    internal WindowExposedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct WindowMovedEvent
{
    private readonly SDL_Event _e;

    internal WindowMovedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct WindowMetalViewResizedEvent
{
    private readonly SDL_Event _e;

    internal WindowMetalViewResizedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct WindowCloseRequestedEvent
{
    private readonly SDL_Event _e;

    internal WindowCloseRequestedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct WindowHitTestEvent
{
    private readonly SDL_Event _e;

    internal WindowHitTestEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct WindowIccProfChangedEvent
{
    private readonly SDL_Event _e;

    internal WindowIccProfChangedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct WindowDisplayChangedEvent
{
    private readonly SDL_Event _e;

    internal WindowDisplayChangedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct WindowDisplayScaleChangedEvent
{
    private readonly SDL_Event _e;

    internal WindowDisplayScaleChangedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct WindowSafeAreaChangedEvent
{
    private readonly SDL_Event _e;

    internal WindowSafeAreaChangedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct WindowOccludedEvent
{
    private readonly SDL_Event _e;

    internal WindowOccludedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct WindowDestroyedEvent
{
    private readonly SDL_Event _e;

    internal WindowDestroyedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct WindowHdrStateChangedEvent
{
    private readonly SDL_Event _e;

    internal WindowHdrStateChangedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct KeymapChangedEvent
{
    private readonly SDL_Event _e;

    internal KeymapChangedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct KeyboardAddedEvent
{
    private readonly SDL_Event _e;

    internal KeyboardAddedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct KeyboardRemovedEvent
{
    private readonly SDL_Event _e;

    internal KeyboardRemovedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct ScreenKeyboardShownEvent
{
    private readonly SDL_Event _e;

    internal ScreenKeyboardShownEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct ScreenKeyboardHiddenEvent
{
    private readonly SDL_Event _e;

    internal ScreenKeyboardHiddenEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct MouseAddedEvent
{
    private readonly SDL_Event _e;

    internal MouseAddedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct MouseRemovedEvent
{
    private readonly SDL_Event _e;

    internal MouseRemovedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct JoystickAxisMotionEvent
{
    private readonly SDL_Event _e;

    internal JoystickAxisMotionEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct JoystickBallMotionEvent
{
    private readonly SDL_Event _e;

    internal JoystickBallMotionEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct JoystickHatMotionEvent
{
    private readonly SDL_Event _e;

    internal JoystickHatMotionEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct JoystickButtonDownEvent
{
    private readonly SDL_Event _e;

    internal JoystickButtonDownEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct JoystickButtonUpEvent
{
    private readonly SDL_Event _e;

    internal JoystickButtonUpEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct JoystickAddedEvent
{
    private readonly SDL_Event _e;

    internal JoystickAddedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct JoystickRemovedEvent
{
    private readonly SDL_Event _e;

    internal JoystickRemovedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct JoystickBatteryUpdatedEvent
{
    private readonly SDL_Event _e;

    internal JoystickBatteryUpdatedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct JoystickUpdateCompleteEvent
{
    private readonly SDL_Event _e;

    internal JoystickUpdateCompleteEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct GamepadAxisMotionEvent
{
    private readonly SDL_Event _e;

    internal GamepadAxisMotionEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct GamepadButtonDownEvent
{
    private readonly SDL_Event _e;

    internal GamepadButtonDownEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct GamepadButtonUpEvent
{
    private readonly SDL_Event _e;

    internal GamepadButtonUpEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct GamepadAddedEvent
{
    private readonly SDL_Event _e;

    internal GamepadAddedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct GamepadRemovedEvent
{
    private readonly SDL_Event _e;

    internal GamepadRemovedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct GamepadRemappedEvent
{
    private readonly SDL_Event _e;

    internal GamepadRemappedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct GamepadTouchpadDownEvent
{
    private readonly SDL_Event _e;

    internal GamepadTouchpadDownEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct GamepadTouchpadMotionEvent
{
    private readonly SDL_Event _e;

    internal GamepadTouchpadMotionEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct GamepadTouchpadUpEvent
{
    private readonly SDL_Event _e;

    internal GamepadTouchpadUpEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct GamepadSensorUpdateEvent
{
    private readonly SDL_Event _e;

    internal GamepadSensorUpdateEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct GamepadUpdateCompleteEvent
{
    private readonly SDL_Event _e;

    internal GamepadUpdateCompleteEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct GamepadSteamHandleUpdatedEvent
{
    private readonly SDL_Event _e;

    internal GamepadSteamHandleUpdatedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct FingerDownEvent
{
    private readonly SDL_Event _e;

    internal FingerDownEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct FingerUpEvent
{
    private readonly SDL_Event _e;

    internal FingerUpEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct FingerMotionEvent
{
    private readonly SDL_Event _e;

    internal FingerMotionEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct FingerCanceledEvent
{
    private readonly SDL_Event _e;

    internal FingerCanceledEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct PinchBeginEvent
{
    private readonly SDL_Event _e;

    internal PinchBeginEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct PinchUpdateEvent
{
    private readonly SDL_Event _e;

    internal PinchUpdateEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct PinchEndEvent
{
    private readonly SDL_Event _e;

    internal PinchEndEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct ClipboardUpdateEvent
{
    private readonly SDL_Event _e;

    internal ClipboardUpdateEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct DropFileEvent
{
    private readonly SDL_Event _e;

    internal DropFileEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct DropTextEvent
{
    private readonly SDL_Event _e;

    internal DropTextEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct DropBeginEvent
{
    private readonly SDL_Event _e;

    internal DropBeginEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct DropCompleteEvent
{
    private readonly SDL_Event _e;

    internal DropCompleteEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct DropPositionEvent
{
    private readonly SDL_Event _e;

    internal DropPositionEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct AudioDeviceAddedEvent
{
    private readonly SDL_Event _e;

    internal AudioDeviceAddedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct AudioDeviceRemovedEvent
{
    private readonly SDL_Event _e;

    internal AudioDeviceRemovedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct AudioDeviceFormatChangedEvent
{
    private readonly SDL_Event _e;

    internal AudioDeviceFormatChangedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct SensorUpdateEvent
{
    private readonly SDL_Event _e;

    internal SensorUpdateEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct PenProximityInEvent
{
    private readonly SDL_Event _e;

    internal PenProximityInEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct PenProximityOutEvent
{
    private readonly SDL_Event _e;

    internal PenProximityOutEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct PenDownEvent
{
    private readonly SDL_Event _e;

    internal PenDownEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct PenUpEvent
{
    private readonly SDL_Event _e;

    internal PenUpEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct PenButtonDownEvent
{
    private readonly SDL_Event _e;

    internal PenButtonDownEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct PenButtonUpEvent
{
    private readonly SDL_Event _e;

    internal PenButtonUpEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct PenMotionEvent
{
    private readonly SDL_Event _e;

    internal PenMotionEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct PenAxisEvent
{
    private readonly SDL_Event _e;

    internal PenAxisEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct CameraDeviceAddedEvent
{
    private readonly SDL_Event _e;

    internal CameraDeviceAddedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct CameraDeviceRemovedEvent
{
    private readonly SDL_Event _e;

    internal CameraDeviceRemovedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct CameraDeviceApprovedEvent
{
    private readonly SDL_Event _e;

    internal CameraDeviceApprovedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct CameraDeviceDeniedEvent
{
    private readonly SDL_Event _e;

    internal CameraDeviceDeniedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct RenderTargetsResetEvent
{
    private readonly SDL_Event _e;

    internal RenderTargetsResetEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct RenderDeviceResetEvent
{
    private readonly SDL_Event _e;

    internal RenderDeviceResetEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct RenderDeviceLostEvent
{
    private readonly SDL_Event _e;

    internal RenderDeviceLostEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct Private0Event
{
    private readonly SDL_Event _e;

    internal Private0Event(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct Private1Event
{
    private readonly SDL_Event _e;

    internal Private1Event(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct Private2Event
{
    private readonly SDL_Event _e;

    internal Private2Event(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct Private3Event
{
    private readonly SDL_Event _e;

    internal Private3Event(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct PollSentinelEvent
{
    private readonly SDL_Event _e;

    internal PollSentinelEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct UserEvent
{
    private readonly SDL_Event _e;

    internal UserEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct DidEnterBackgroundEvent
{
    private readonly SDL_Event _e;

    internal DidEnterBackgroundEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}

public readonly struct WillEnterForegroundEvent
{
    private readonly SDL_Event _e;

    internal WillEnterForegroundEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}