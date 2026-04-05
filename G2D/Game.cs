using SDL;

namespace G2D;

public abstract unsafe class Game
{
    private bool _running;

    internal static EmbeddedResource InternalResource { get; } = new(typeof(Game).Assembly);

    protected Keyboard Keyboard { get; private set; } = null!;

    protected Mouse Mouse { get; private set; } = null!;

    protected GamePad GamePad { get; private set; } = null!;

    protected EmbeddedResource Resource { get; private set; } = null!;

    protected StepTimer Timer { get; private set; } = null!;

    protected Window Window { get; private set; } = null!;

    internal VulkanContext VulkanContext { get; private set; } = null!;

    public void Launch(string[]? args = null)
    {
        Initialize();
        Run();
        Cleanup();
    }

    internal void Initialize()
    {
        var config = new Config();
        Config(config);

        Resource = new EmbeddedResource(GetType().Assembly);

        Window = new Window(
            config.WindowTitle, config.WindowWidth, config.WindowHeight,
            (config.WindowResizable ? WindowFlags.Resizable : WindowFlags.None)
            | (config.WindowBorderless ? WindowFlags.Borderless : WindowFlags.None)
            | (config.WindowFullscreen ? WindowFlags.Fullscreen : WindowFlags.None)
        );

        if (config.VSync)
        {
            var rate = Window.GetDisplayRefreshRate();
            Timer = new StepTimer(rate);
        }
        else
        {
            Timer = new StepTimer(config.TargetFps);
        }

        VulkanContext = new VulkanContext(
            Window,
            config.ApplicationName, config.ApplicationVersion,
            config.EngineName, config.EngineVersion,
            config.DebugMode);

        Keyboard = new Keyboard();
        Mouse = new Mouse();
        GamePad = new GamePad();
    }

    internal void Cleanup()
    {
        ((IDisposable)VulkanContext).Dispose();
        ((IDisposable)Window).Dispose();
    }


    internal void Run()
    {
        InternalLoad();

        _running = true;
        SDL_Event e = new();
        while (_running)
        {
            while (SDL3.SDL_PollEvent(&e)) InternalEvent(ref e);
            if (!_running) break;

            InternalIteration();
        }

        InternalUnload();
    }

    protected void Exit()
    {
        _running = false;
    }

    internal void InternalLoad()
    {
        Load();
        Timer.Start();
        Window.Show();
    }

    internal void InternalUnload()
    {
        Timer.Stop();
        Unload();
    }

    internal void InternalIteration()
    {
        Timer.WaitTargetFps();

        // internal update
        Timer.Step();

        // user update
        Update(Timer.GetDeltaTime());

        var g = VulkanContext.StartDrawSession();
        if (g == null) return;

        g.UniformData.Time = Timer.GetTimeF();
        g.UniformData.MousePosition = Mouse.GetPosition();

        Draw(g);

        VulkanContext.EndDrawSession(g);
    }

    internal void InternalEvent(ref SDL_Event e)
    {
        switch (e.Type)
        {
            case SDL_EventType.SDL_EVENT_QUIT:
                var quitEvent = new QuitEvent(ref e);
                Event(ref quitEvent);
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
                Event(ref displayEvent);
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
                Event(ref windowEvent);
                break;
            case SDL_EventType.SDL_EVENT_KEY_DOWN:
            case SDL_EventType.SDL_EVENT_KEY_UP:
                var keyboardEvent = new KeyboardEvent(ref e);
                Event(ref keyboardEvent);
                break;
            case SDL_EventType.SDL_EVENT_TEXT_EDITING:
                var textEditingEvent = new TextEditingEvent(ref e);
                Event(ref textEditingEvent);
                break;
            case SDL_EventType.SDL_EVENT_TEXT_INPUT:
                var textInputEvent = new TextInputEvent(ref e);
                Event(ref textInputEvent);
                break;
            case SDL_EventType.SDL_EVENT_KEYBOARD_ADDED:
            case SDL_EventType.SDL_EVENT_KEYBOARD_REMOVED:
                var keyboardDeviceEvent = new KeyboardDeviceEvent(ref e);
                Event(ref keyboardDeviceEvent);
                break;
            case SDL_EventType.SDL_EVENT_TEXT_EDITING_CANDIDATES:
                var textEditingCandidatesEvent = new TextEditingCandidatesEvent(ref e);
                Event(ref textEditingCandidatesEvent);
                break;
            case SDL_EventType.SDL_EVENT_MOUSE_MOTION:
                var mouseMotionEvent = new MouseMotionEvent(ref e);
                Event(ref mouseMotionEvent);
                break;
            case SDL_EventType.SDL_EVENT_MOUSE_BUTTON_DOWN:
            case SDL_EventType.SDL_EVENT_MOUSE_BUTTON_UP:
                var mouseButtonEvent = new MouseButtonEvent(ref e);
                Event(ref mouseButtonEvent);
                break;
            case SDL_EventType.SDL_EVENT_MOUSE_WHEEL:
                var mouseWheelEvent = new MouseWheelEvent(ref e);
                Event(ref mouseWheelEvent);
                break;
            case SDL_EventType.SDL_EVENT_MOUSE_ADDED:
            case SDL_EventType.SDL_EVENT_MOUSE_REMOVED:
                var mouseDeviceEvent = new MouseDeviceEvent(ref e);
                Event(ref mouseDeviceEvent);
                break;
            case SDL_EventType.SDL_EVENT_JOYSTICK_AXIS_MOTION:
                var joyAxisEvent = new JoyAxisEvent(ref e);
                Event(ref joyAxisEvent);
                break;
            case SDL_EventType.SDL_EVENT_JOYSTICK_BALL_MOTION:
                var joyBallEvent = new JoyBallEvent(ref e);
                Event(ref joyBallEvent);
                break;
            case SDL_EventType.SDL_EVENT_JOYSTICK_HAT_MOTION:
                var joyHatEvent = new JoyHatEvent(ref e);
                Event(ref joyHatEvent);
                break;
            case SDL_EventType.SDL_EVENT_JOYSTICK_BUTTON_DOWN:
            case SDL_EventType.SDL_EVENT_JOYSTICK_BUTTON_UP:
                var joyButtonEvent = new JoyButtonEvent(ref e);
                Event(ref joyButtonEvent);
                break;
            case SDL_EventType.SDL_EVENT_JOYSTICK_ADDED:
            case SDL_EventType.SDL_EVENT_JOYSTICK_REMOVED:
            case SDL_EventType.SDL_EVENT_JOYSTICK_UPDATE_COMPLETE:
                var joyDeviceEvent = new JoyDeviceEvent(ref e);
                Event(ref joyDeviceEvent);
                break;
            case SDL_EventType.SDL_EVENT_JOYSTICK_BATTERY_UPDATED:
                var joyBatteryEvent = new JoyBatteryEvent(ref e);
                Event(ref joyBatteryEvent);
                break;
            case SDL_EventType.SDL_EVENT_GAMEPAD_AXIS_MOTION:
                var gamepadAxisEvent = new GamepadAxisEvent(ref e);
                Event(ref gamepadAxisEvent);
                break;
            case SDL_EventType.SDL_EVENT_GAMEPAD_BUTTON_DOWN:
            case SDL_EventType.SDL_EVENT_GAMEPAD_BUTTON_UP:
                var gamepadButtonEvent = new GamepadButtonEvent(ref e);
                Event(ref gamepadButtonEvent);
                break;
            case SDL_EventType.SDL_EVENT_GAMEPAD_ADDED:
            case SDL_EventType.SDL_EVENT_GAMEPAD_REMOVED:
            case SDL_EventType.SDL_EVENT_GAMEPAD_REMAPPED:
            case SDL_EventType.SDL_EVENT_GAMEPAD_UPDATE_COMPLETE:
            case SDL_EventType.SDL_EVENT_GAMEPAD_STEAM_HANDLE_UPDATED:
                var gamepadDeviceEvent = new GamepadDeviceEvent(ref e);
                Event(ref gamepadDeviceEvent);
                break;
            case SDL_EventType.SDL_EVENT_GAMEPAD_TOUCHPAD_DOWN:
            case SDL_EventType.SDL_EVENT_GAMEPAD_TOUCHPAD_MOTION:
            case SDL_EventType.SDL_EVENT_GAMEPAD_TOUCHPAD_UP:
                var gamepadTouchpadEvent = new GamepadTouchpadEvent(ref e);
                Event(ref gamepadTouchpadEvent);
                break;
            case SDL_EventType.SDL_EVENT_GAMEPAD_SENSOR_UPDATE:
                var gamepadSensorEvent = new GamepadSensorEvent(ref e);
                Event(ref gamepadSensorEvent);
                break;
            case SDL_EventType.SDL_EVENT_FINGER_DOWN:
            case SDL_EventType.SDL_EVENT_FINGER_UP:
            case SDL_EventType.SDL_EVENT_FINGER_MOTION:
            case SDL_EventType.SDL_EVENT_FINGER_CANCELED:
                var touchFingerEvent = new TouchFingerEvent(ref e);
                Event(ref touchFingerEvent);
                break;
            case SDL_EventType.SDL_EVENT_PINCH_BEGIN:
            case SDL_EventType.SDL_EVENT_PINCH_UPDATE:
            case SDL_EventType.SDL_EVENT_PINCH_END:
                var pinchFingerEvent = new PinchFingerEvent(ref e);
                Event(ref pinchFingerEvent);
                break;
            case SDL_EventType.SDL_EVENT_CLIPBOARD_UPDATE:
                var clipboardEvent = new ClipboardEvent(ref e);
                Event(ref clipboardEvent);
                break;
            case SDL_EventType.SDL_EVENT_DROP_FILE:
            case SDL_EventType.SDL_EVENT_DROP_TEXT:
            case SDL_EventType.SDL_EVENT_DROP_BEGIN:
            case SDL_EventType.SDL_EVENT_DROP_COMPLETE:
            case SDL_EventType.SDL_EVENT_DROP_POSITION:
                var dropEvent = new DropEvent(ref e);
                Event(ref dropEvent);
                break;
            case SDL_EventType.SDL_EVENT_AUDIO_DEVICE_ADDED:
            case SDL_EventType.SDL_EVENT_AUDIO_DEVICE_REMOVED:
            case SDL_EventType.SDL_EVENT_AUDIO_DEVICE_FORMAT_CHANGED:
                var audioDeviceEvent = new AudioDeviceEvent(ref e);
                Event(ref audioDeviceEvent);
                break;
            case SDL_EventType.SDL_EVENT_SENSOR_UPDATE:
                var sensorEvent = new SensorEvent(ref e);
                Event(ref sensorEvent);
                break;
            case SDL_EventType.SDL_EVENT_PEN_PROXIMITY_IN:
            case SDL_EventType.SDL_EVENT_PEN_PROXIMITY_OUT:
                var penProximityEvent = new PenProximityEvent(ref e);
                Event(ref penProximityEvent);
                break;
            case SDL_EventType.SDL_EVENT_PEN_DOWN:
            case SDL_EventType.SDL_EVENT_PEN_UP:
                var penTouchEvent = new PenTouchEvent(ref e);
                Event(ref penTouchEvent);
                break;
            case SDL_EventType.SDL_EVENT_PEN_BUTTON_DOWN:
            case SDL_EventType.SDL_EVENT_PEN_BUTTON_UP:
                var penButtonEvent = new PenButtonEvent(ref e);
                Event(ref penButtonEvent);
                break;
            case SDL_EventType.SDL_EVENT_PEN_MOTION:
                var penMotionEvent = new PenMotionEvent(ref e);
                Event(ref penMotionEvent);
                break;
            case SDL_EventType.SDL_EVENT_PEN_AXIS:
                var penAxisEvent = new PenAxisEvent(ref e);
                Event(ref penAxisEvent);
                break;
            case SDL_EventType.SDL_EVENT_CAMERA_DEVICE_ADDED:
            case SDL_EventType.SDL_EVENT_CAMERA_DEVICE_REMOVED:
            case SDL_EventType.SDL_EVENT_CAMERA_DEVICE_APPROVED:
            case SDL_EventType.SDL_EVENT_CAMERA_DEVICE_DENIED:
                var cameraDeviceEvent = new CameraDeviceEvent(ref e);
                Event(ref cameraDeviceEvent);
                break;
            case SDL_EventType.SDL_EVENT_RENDER_TARGETS_RESET:
            case SDL_EventType.SDL_EVENT_RENDER_DEVICE_RESET:
            case SDL_EventType.SDL_EVENT_RENDER_DEVICE_LOST:
                var renderEvent = new RenderEvent(ref e);
                Event(ref renderEvent);
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
                    Event(ref userEvent);
                }

                // TODO
                break;
        }
    }

    protected virtual void Config(Config config)
    {
    }

    protected virtual void Load()
    {
    }

    protected virtual void Update(double dt)
    {
    }

    protected virtual void Draw(Graphics graphics)
    {
    }

    protected virtual void Unload()
    {
    }

    protected virtual void Event(ref DisplayEvent e)
    {
    }

    protected virtual void Event(ref WindowEvent e)
    {
    }

    protected virtual void Event(ref KeyboardDeviceEvent e)
    {
    }

    protected virtual void Event(ref KeyboardEvent e)
    {
    }

    protected virtual void Event(ref TextEditingEvent e)
    {
    }

    protected virtual void Event(ref TextEditingCandidatesEvent e)
    {
    }

    protected virtual void Event(ref TextInputEvent e)
    {
    }

    protected virtual void Event(ref MouseDeviceEvent e)
    {
    }

    protected virtual void Event(ref MouseMotionEvent e)
    {
    }

    protected virtual void Event(ref MouseButtonEvent e)
    {
    }

    protected virtual void Event(ref MouseWheelEvent e)
    {
    }

    protected virtual void Event(ref JoyDeviceEvent e)
    {
    }

    protected virtual void Event(ref JoyAxisEvent e)
    {
    }

    protected virtual void Event(ref JoyBallEvent e)
    {
    }

    protected virtual void Event(ref JoyHatEvent e)
    {
    }

    protected virtual void Event(ref JoyButtonEvent e)
    {
    }

    protected virtual void Event(ref JoyBatteryEvent e)
    {
    }

    protected virtual void Event(ref GamepadDeviceEvent e)
    {
    }

    protected virtual void Event(ref GamepadAxisEvent e)
    {
    }

    protected virtual void Event(ref GamepadButtonEvent e)
    {
    }

    protected virtual void Event(ref GamepadTouchpadEvent e)
    {
    }

    protected virtual void Event(ref GamepadSensorEvent e)
    {
    }

    protected virtual void Event(ref AudioDeviceEvent e)
    {
    }

    protected virtual void Event(ref CameraDeviceEvent e)
    {
    }

    protected virtual void Event(ref SensorEvent e)
    {
    }

    protected virtual void Event(ref QuitEvent e)
    {
        Exit();
    }

    protected virtual void Event(ref UserEvent e)
    {
    }

    protected virtual void Event(ref TouchFingerEvent e)
    {
    }

    protected virtual void Event(ref PinchFingerEvent e)
    {
    }

    protected virtual void Event(ref PenProximityEvent e)
    {
    }

    protected virtual void Event(ref PenTouchEvent e)
    {
    }

    protected virtual void Event(ref PenMotionEvent e)
    {
    }

    protected virtual void Event(ref PenButtonEvent e)
    {
    }

    protected virtual void Event(ref PenAxisEvent e)
    {
    }

    protected virtual void Event(ref RenderEvent e)
    {
    }

    protected virtual void Event(ref DropEvent e)
    {
    }

    protected virtual void Event(ref ClipboardEvent e)
    {
    }
}