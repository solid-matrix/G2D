using SDL;

namespace G2D;

public class Events
{
    public delegate void EventAction<T>(ref T e) where T : allows ref struct;

    public event EventAction<DisplayEvent>? OnDisplayEvent;

    public event EventAction<WindowEvent>? OnWindowEvent;

    public event EventAction<KeyboardDeviceEvent>? OnKeyboardDeviceEvent;

    public event EventAction<KeyboardEvent>? OnKeyboardEvent;

    public event EventAction<TextEditingEvent>? OnTextEditingEvent;

    public event EventAction<TextEditingCandidatesEvent>? OnTextEditingCandidatesEvent;

    public event EventAction<TextInputEvent>? OnTextInputEvent;

    public event EventAction<MouseDeviceEvent>? OnMouseDeviceEvent;

    public event EventAction<MouseMotionEvent>? OnMouseMotionEvent;

    public event EventAction<MouseButtonEvent>? OnMouseButtonEvent;

    public event EventAction<MouseWheelEvent>? OnMouseWheelEvent;

    public event EventAction<JoyDeviceEvent>? OnJoyDeviceEvent;

    public event EventAction<JoyAxisEvent>? OnJoyAxisEvent;

    public event EventAction<JoyBallEvent>? OnJoyBallEvent;

    public event EventAction<JoyHatEvent>? OnJoyHatEvent;

    public event EventAction<JoyButtonEvent>? OnJoyButtonEvent;

    public event EventAction<JoyBatteryEvent>? OnJoyBatteryEvent;

    public event EventAction<GamepadDeviceEvent>? OnGamepadDeviceEvent;

    public event EventAction<GamepadAxisEvent>? OnGamepadAxisEvent;

    public event EventAction<GamepadButtonEvent>? OnGamepadButtonEvent;

    public event EventAction<GamepadTouchpadEvent>? OnGamepadTouchpadEvent;

    public event EventAction<GamepadSensorEvent>? OnGamepadSensorEvent;

    public event EventAction<AudioDeviceEvent>? OnAudioDeviceEvent;

    public event EventAction<CameraDeviceEvent>? OnCameraDeviceEvent;

    public event EventAction<SensorEvent>? OnSensorEvent;

    public event EventAction<QuitEvent>? OnQuitEvent;

    public event EventAction<UserEvent>? OnUserEvent;

    public event EventAction<TouchFingerEvent>? OnTouchFingerEvent;

    public event EventAction<PinchFingerEvent>? OnPinchFingerEvent;

    public event EventAction<PenProximityEvent>? OnPenProximityEvent;

    public event EventAction<PenTouchEvent>? OnPenTouchEvent;

    public event EventAction<PenMotionEvent>? OnPenMotionEvent;

    public event EventAction<PenButtonEvent>? OnPenButtonEvent;

    public event EventAction<PenAxisEvent>? OnPenAxisEvent;

    public event EventAction<RenderEvent>? OnRenderEvent;

    public event EventAction<DropEvent>? OnDropEvent;

    public event EventAction<ClipboardEvent>? OnClipboardEvent;

    internal void Process(ref SDL_Event e)
    {
        switch (e.Type)
        {
            case SDL_EventType.SDL_EVENT_QUIT:
                var quitEvent = new QuitEvent(ref e);
                OnQuitEvent?.Invoke(ref quitEvent);
                break;
            case SDL_EventType.SDL_EVENT_DISPLAY_ORIENTATION:
            case SDL_EventType.SDL_EVENT_DISPLAY_ADDED:
            case SDL_EventType.SDL_EVENT_DISPLAY_REMOVED:
            case SDL_EventType.SDL_EVENT_DISPLAY_MOVED:
            case SDL_EventType.SDL_EVENT_DISPLAY_DESKTOP_MODE_CHANGED:
            case SDL_EventType.SDL_EVENT_DISPLAY_CURRENT_MODE_CHANGED:
            case SDL_EventType.SDL_EVENT_DISPLAY_CONTENT_SCALE_CHANGED:
            case SDL_EventType.SDL_EVENT_DISPLAY_USABLE_BOUNDS_CHANGED:
                var displayEvent = new DisplayEvent(ref e);
                OnDisplayEvent?.Invoke(ref displayEvent);
                break;
            case SDL_EventType.SDL_EVENT_WINDOW_SHOWN:
            case SDL_EventType.SDL_EVENT_WINDOW_HIDDEN:
            case SDL_EventType.SDL_EVENT_WINDOW_EXPOSED:
            case SDL_EventType.SDL_EVENT_WINDOW_MOVED:
            case SDL_EventType.SDL_EVENT_WINDOW_RESIZED:
            case SDL_EventType.SDL_EVENT_WINDOW_PIXEL_SIZE_CHANGED:
            case SDL_EventType.SDL_EVENT_WINDOW_METAL_VIEW_RESIZED:
            case SDL_EventType.SDL_EVENT_WINDOW_MINIMIZED:
            case SDL_EventType.SDL_EVENT_WINDOW_MAXIMIZED:
            case SDL_EventType.SDL_EVENT_WINDOW_RESTORED:
            case SDL_EventType.SDL_EVENT_WINDOW_MOUSE_ENTER:
            case SDL_EventType.SDL_EVENT_WINDOW_MOUSE_LEAVE:
            case SDL_EventType.SDL_EVENT_WINDOW_FOCUS_GAINED:
            case SDL_EventType.SDL_EVENT_WINDOW_FOCUS_LOST:
            case SDL_EventType.SDL_EVENT_WINDOW_CLOSE_REQUESTED:
            case SDL_EventType.SDL_EVENT_WINDOW_HIT_TEST:
            case SDL_EventType.SDL_EVENT_WINDOW_ICCPROF_CHANGED:
            case SDL_EventType.SDL_EVENT_WINDOW_DISPLAY_CHANGED:
            case SDL_EventType.SDL_EVENT_WINDOW_DISPLAY_SCALE_CHANGED:
            case SDL_EventType.SDL_EVENT_WINDOW_SAFE_AREA_CHANGED:
            case SDL_EventType.SDL_EVENT_WINDOW_OCCLUDED:
            case SDL_EventType.SDL_EVENT_WINDOW_ENTER_FULLSCREEN:
            case SDL_EventType.SDL_EVENT_WINDOW_LEAVE_FULLSCREEN:
            case SDL_EventType.SDL_EVENT_WINDOW_DESTROYED:
            case SDL_EventType.SDL_EVENT_WINDOW_HDR_STATE_CHANGED:
                var windowEvent = new WindowEvent(ref e);
                OnWindowEvent?.Invoke(ref windowEvent);
                break;
            case SDL_EventType.SDL_EVENT_KEY_DOWN:
            case SDL_EventType.SDL_EVENT_KEY_UP:
                var keyboardEvent = new KeyboardEvent(ref e);
                OnKeyboardEvent?.Invoke(ref keyboardEvent);
                break;
            case SDL_EventType.SDL_EVENT_TEXT_EDITING:
                var textEditingEvent = new TextEditingEvent(ref e);
                OnTextEditingEvent?.Invoke(ref textEditingEvent);
                break;
            case SDL_EventType.SDL_EVENT_TEXT_INPUT:
                var textInputEvent = new TextInputEvent(ref e);
                OnTextInputEvent?.Invoke(ref textInputEvent);
                break;
            case SDL_EventType.SDL_EVENT_KEYBOARD_ADDED:
            case SDL_EventType.SDL_EVENT_KEYBOARD_REMOVED:
                var keyboardDeviceEvent = new KeyboardDeviceEvent(ref e);
                OnKeyboardDeviceEvent?.Invoke(ref keyboardDeviceEvent);
                break;
            case SDL_EventType.SDL_EVENT_TEXT_EDITING_CANDIDATES:
                var textEditingCandidatesEvent = new TextEditingCandidatesEvent(ref e);
                OnTextEditingCandidatesEvent?.Invoke(ref textEditingCandidatesEvent);
                break;
            case SDL_EventType.SDL_EVENT_MOUSE_MOTION:
                var mouseMotionEvent = new MouseMotionEvent(ref e);
                OnMouseMotionEvent?.Invoke(ref mouseMotionEvent);
                break;
            case SDL_EventType.SDL_EVENT_MOUSE_BUTTON_DOWN:
            case SDL_EventType.SDL_EVENT_MOUSE_BUTTON_UP:
                var mouseButtonEvent = new MouseButtonEvent(ref e);
                OnMouseButtonEvent?.Invoke(ref mouseButtonEvent);
                break;
            case SDL_EventType.SDL_EVENT_MOUSE_WHEEL:
                var mouseWheelEvent = new MouseWheelEvent(ref e);
                OnMouseWheelEvent?.Invoke(ref mouseWheelEvent);
                break;
            case SDL_EventType.SDL_EVENT_MOUSE_ADDED:
            case SDL_EventType.SDL_EVENT_MOUSE_REMOVED:
                var mouseDeviceEvent = new MouseDeviceEvent(ref e);
                OnMouseDeviceEvent?.Invoke(ref mouseDeviceEvent);
                break;
            case SDL_EventType.SDL_EVENT_JOYSTICK_AXIS_MOTION:
                var joyAxisEvent = new JoyAxisEvent(ref e);
                OnJoyAxisEvent?.Invoke(ref joyAxisEvent);
                break;
            case SDL_EventType.SDL_EVENT_JOYSTICK_BALL_MOTION:
                var joyBallEvent = new JoyBallEvent(ref e);
                OnJoyBallEvent?.Invoke(ref joyBallEvent);
                break;
            case SDL_EventType.SDL_EVENT_JOYSTICK_HAT_MOTION:
                var joyHatEvent = new JoyHatEvent(ref e);
                OnJoyHatEvent?.Invoke(ref joyHatEvent);
                break;
            case SDL_EventType.SDL_EVENT_JOYSTICK_BUTTON_DOWN:
            case SDL_EventType.SDL_EVENT_JOYSTICK_BUTTON_UP:
                var joyButtonEvent = new JoyButtonEvent(ref e);
                OnJoyButtonEvent?.Invoke(ref joyButtonEvent);
                break;
            case SDL_EventType.SDL_EVENT_JOYSTICK_ADDED:
            case SDL_EventType.SDL_EVENT_JOYSTICK_REMOVED:
            case SDL_EventType.SDL_EVENT_JOYSTICK_UPDATE_COMPLETE:
                var joyDeviceEvent = new JoyDeviceEvent(ref e);
                OnJoyDeviceEvent?.Invoke(ref joyDeviceEvent);
                break;
            case SDL_EventType.SDL_EVENT_JOYSTICK_BATTERY_UPDATED:
                var joyBatteryEvent = new JoyBatteryEvent(ref e);
                OnJoyBatteryEvent?.Invoke(ref joyBatteryEvent);
                break;
            case SDL_EventType.SDL_EVENT_GAMEPAD_AXIS_MOTION:
                var gamepadAxisEvent = new GamepadAxisEvent(ref e);
                OnGamepadAxisEvent?.Invoke(ref gamepadAxisEvent);
                break;
            case SDL_EventType.SDL_EVENT_GAMEPAD_BUTTON_DOWN:
            case SDL_EventType.SDL_EVENT_GAMEPAD_BUTTON_UP:
                var gamepadButtonEvent = new GamepadButtonEvent(ref e);
                OnGamepadButtonEvent?.Invoke(ref gamepadButtonEvent);
                break;
            case SDL_EventType.SDL_EVENT_GAMEPAD_ADDED:
            case SDL_EventType.SDL_EVENT_GAMEPAD_REMOVED:
            case SDL_EventType.SDL_EVENT_GAMEPAD_REMAPPED:
            case SDL_EventType.SDL_EVENT_GAMEPAD_UPDATE_COMPLETE:
            case SDL_EventType.SDL_EVENT_GAMEPAD_STEAM_HANDLE_UPDATED:
                var gamepadDeviceEvent = new GamepadDeviceEvent(ref e);
                OnGamepadDeviceEvent?.Invoke(ref gamepadDeviceEvent);
                break;
            case SDL_EventType.SDL_EVENT_GAMEPAD_TOUCHPAD_DOWN:
            case SDL_EventType.SDL_EVENT_GAMEPAD_TOUCHPAD_MOTION:
            case SDL_EventType.SDL_EVENT_GAMEPAD_TOUCHPAD_UP:
                var gamepadTouchpadEvent = new GamepadTouchpadEvent(ref e);
                OnGamepadTouchpadEvent?.Invoke(ref gamepadTouchpadEvent);
                break;
            case SDL_EventType.SDL_EVENT_GAMEPAD_SENSOR_UPDATE:
                var gamepadSensorEvent = new GamepadSensorEvent(ref e);
                OnGamepadSensorEvent?.Invoke(ref gamepadSensorEvent);
                break;
            case SDL_EventType.SDL_EVENT_FINGER_DOWN:
            case SDL_EventType.SDL_EVENT_FINGER_UP:
            case SDL_EventType.SDL_EVENT_FINGER_MOTION:
            case SDL_EventType.SDL_EVENT_FINGER_CANCELED:
                var touchFingerEvent = new TouchFingerEvent(ref e);
                OnTouchFingerEvent?.Invoke(ref touchFingerEvent);
                break;
            case SDL_EventType.SDL_EVENT_PINCH_BEGIN:
            case SDL_EventType.SDL_EVENT_PINCH_UPDATE:
            case SDL_EventType.SDL_EVENT_PINCH_END:
                var pinchFingerEvent = new PinchFingerEvent(ref e);
                OnPinchFingerEvent?.Invoke(ref pinchFingerEvent);
                break;
            case SDL_EventType.SDL_EVENT_CLIPBOARD_UPDATE:
                var clipboardEvent = new ClipboardEvent(ref e);
                OnClipboardEvent?.Invoke(ref clipboardEvent);
                break;
            case SDL_EventType.SDL_EVENT_DROP_FILE:
            case SDL_EventType.SDL_EVENT_DROP_TEXT:
            case SDL_EventType.SDL_EVENT_DROP_BEGIN:
            case SDL_EventType.SDL_EVENT_DROP_COMPLETE:
            case SDL_EventType.SDL_EVENT_DROP_POSITION:
                var dropEvent = new DropEvent(ref e);
                OnDropEvent?.Invoke(ref dropEvent);
                break;
            case SDL_EventType.SDL_EVENT_AUDIO_DEVICE_ADDED:
            case SDL_EventType.SDL_EVENT_AUDIO_DEVICE_REMOVED:
            case SDL_EventType.SDL_EVENT_AUDIO_DEVICE_FORMAT_CHANGED:
                var audioDeviceEvent = new AudioDeviceEvent(ref e);
                OnAudioDeviceEvent?.Invoke(ref audioDeviceEvent);
                break;
            case SDL_EventType.SDL_EVENT_SENSOR_UPDATE:
                var sensorEvent = new SensorEvent(ref e);
                OnSensorEvent?.Invoke(ref sensorEvent);
                break;
            case SDL_EventType.SDL_EVENT_PEN_PROXIMITY_IN:
            case SDL_EventType.SDL_EVENT_PEN_PROXIMITY_OUT:
                var penProximityEvent = new PenProximityEvent(ref e);
                OnPenProximityEvent?.Invoke(ref penProximityEvent);
                break;
            case SDL_EventType.SDL_EVENT_PEN_DOWN:
            case SDL_EventType.SDL_EVENT_PEN_UP:
                var penTouchEvent = new PenTouchEvent(ref e);
                OnPenTouchEvent?.Invoke(ref penTouchEvent);
                break;
            case SDL_EventType.SDL_EVENT_PEN_BUTTON_DOWN:
            case SDL_EventType.SDL_EVENT_PEN_BUTTON_UP:
                var penButtonEvent = new PenButtonEvent(ref e);
                OnPenButtonEvent?.Invoke(ref penButtonEvent);
                break;
            case SDL_EventType.SDL_EVENT_PEN_MOTION:
                var penMotionEvent = new PenMotionEvent(ref e);
                OnPenMotionEvent?.Invoke(ref penMotionEvent);
                break;
            case SDL_EventType.SDL_EVENT_PEN_AXIS:
                var penAxisEvent = new PenAxisEvent(ref e);
                OnPenAxisEvent?.Invoke(ref penAxisEvent);
                break;
            case SDL_EventType.SDL_EVENT_CAMERA_DEVICE_ADDED:
            case SDL_EventType.SDL_EVENT_CAMERA_DEVICE_REMOVED:
            case SDL_EventType.SDL_EVENT_CAMERA_DEVICE_APPROVED:
            case SDL_EventType.SDL_EVENT_CAMERA_DEVICE_DENIED:
                var cameraDeviceEvent = new CameraDeviceEvent(ref e);
                OnCameraDeviceEvent?.Invoke(ref cameraDeviceEvent);
                break;
            case SDL_EventType.SDL_EVENT_RENDER_TARGETS_RESET:
            case SDL_EventType.SDL_EVENT_RENDER_DEVICE_RESET:
            case SDL_EventType.SDL_EVENT_RENDER_DEVICE_LOST:
                var renderEvent = new RenderEvent(ref e);
                OnRenderEvent?.Invoke(ref renderEvent);
                break;
            case SDL_EventType.SDL_EVENT_KEYMAP_CHANGED:
            case SDL_EventType.SDL_EVENT_SCREEN_KEYBOARD_SHOWN:
            case SDL_EventType.SDL_EVENT_SCREEN_KEYBOARD_HIDDEN:
            case SDL_EventType.SDL_EVENT_TERMINATING:
            case SDL_EventType.SDL_EVENT_LOW_MEMORY:
            case SDL_EventType.SDL_EVENT_WILL_ENTER_BACKGROUND:
            case SDL_EventType.SDL_EVENT_DID_ENTER_BACKGROUND:
            case SDL_EventType.SDL_EVENT_WILL_ENTER_FOREGROUND:
            case SDL_EventType.SDL_EVENT_DID_ENTER_FOREGROUND:
            case SDL_EventType.SDL_EVENT_LOCALE_CHANGED:
            case SDL_EventType.SDL_EVENT_SYSTEM_THEME_CHANGED:
            case SDL_EventType.SDL_EVENT_PRIVATE0:
            case SDL_EventType.SDL_EVENT_PRIVATE1:
            case SDL_EventType.SDL_EVENT_PRIVATE2:
            case SDL_EventType.SDL_EVENT_PRIVATE3:
            case SDL_EventType.SDL_EVENT_POLL_SENTINEL:
            case SDL_EventType.SDL_EVENT_FIRST:
            case SDL_EventType.SDL_EVENT_USER:
            case SDL_EventType.SDL_EVENT_LAST:
            case SDL_EventType.SDL_EVENT_ENUM_PADDING:
            default:
                if (e.Type is >= SDL_EventType.SDL_EVENT_USER and <= SDL_EventType.SDL_EVENT_LAST)
                {
                    var userEvent = new UserEvent(ref e);
                    OnUserEvent?.Invoke(ref userEvent);
                }

                break;
        }
    }
}