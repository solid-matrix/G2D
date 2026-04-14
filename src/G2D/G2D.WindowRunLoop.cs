using System.Runtime.InteropServices;
using SDL;

namespace G2D;

public unsafe partial class G2D
{
    private SDL_Window* _window;

    private KeyboardManager _keyboardManager = null!;

    private MouseManager _mouseManager = null!;

    private void WindowRunLoop()
    {
        // initialize SDL
        if (!SDL3.SDL_Init(SDL_InitFlags.SDL_INIT_VIDEO)) throw new NotSupportedException("SDL not support");

        // create window
        var flags = SDL_WindowFlags.SDL_WINDOW_HIGH_PIXEL_DENSITY
                    | SDL_WindowFlags.SDL_WINDOW_VULKAN
                    | SDL_WindowFlags.SDL_WINDOW_HIDDEN
                    | (_config.WindowResizable ? SDL_WindowFlags.SDL_WINDOW_RESIZABLE : 0)
                    | (_config.WindowBorderless ? SDL_WindowFlags.SDL_WINDOW_BORDERLESS : 0)
                    | (_config.WindowFullscreen ? SDL_WindowFlags.SDL_WINDOW_FULLSCREEN : 0);
        _window = SDL3.SDL_CreateWindow(_config.WindowTitle, _config.WindowWidth, _config.WindowHeight, flags);
        if (_window == null) throw new Exception("SDL: failed to create window; " + SDL3.SDL_GetError());

        _keyboardManager = new KeyboardManager();
        _mouseManager = new MouseManager();

        // get required vulkan instance extensions
        _requiredVulkanInstanceExtensions = GetVulkanInstanceExtensions();

        _initBarrier.SignalAndWait();
        // wait vulkan instance creating
        _initBarrier.SignalAndWait();

        // create vulkan surface
        _vkSurfaceHandle = CreateVulkanSurface(_window, _vkInstanceHandle);
        (_windowWidth, _windowHeight) = GetClientSize(_window);
        _displayRefreshRate = GetDisplayRefreshRate(_window);

        _initBarrier.SignalAndWait();
        // waiting vulkan context creating
        _initBarrier.SignalAndWait();

        SDL3.SDL_ShowWindow(_window);

        SDL_Event e = new();
        while (!ShouldClose)
        {
            while (SDL3.SDL_PollEvent(&e))
            {
                WindowProcessEvent(e);
            }

            if (ShouldClose) break;

            _mouseManager.Update();

            Thread.Sleep(1);
        }

        if (_window != null)
            SDL3.SDL_DestroyWindow(_window);
    }

    private void WindowProcessEvent(in SDL_Event e)
    {
        switch (e.Type)
        {
            case SDL_EventType.SDL_EVENT_WINDOW_SHOWN:
            case SDL_EventType.SDL_EVENT_WINDOW_HIDDEN:
            case SDL_EventType.SDL_EVENT_WINDOW_RESIZED:
            case SDL_EventType.SDL_EVENT_WINDOW_PIXEL_SIZE_CHANGED:
            case SDL_EventType.SDL_EVENT_WINDOW_MINIMIZED:
            case SDL_EventType.SDL_EVENT_WINDOW_MAXIMIZED:
            case SDL_EventType.SDL_EVENT_WINDOW_RESTORED:
            case SDL_EventType.SDL_EVENT_WINDOW_MOUSE_ENTER:
            case SDL_EventType.SDL_EVENT_WINDOW_MOUSE_LEAVE:
            case SDL_EventType.SDL_EVENT_WINDOW_FOCUS_GAINED:
            case SDL_EventType.SDL_EVENT_WINDOW_FOCUS_LOST:
            case SDL_EventType.SDL_EVENT_WINDOW_ENTER_FULLSCREEN:
            case SDL_EventType.SDL_EVENT_WINDOW_LEAVE_FULLSCREEN:
            case SDL_EventType.SDL_EVENT_KEY_DOWN:
            case SDL_EventType.SDL_EVENT_KEY_UP:
            case SDL_EventType.SDL_EVENT_TEXT_EDITING:
            case SDL_EventType.SDL_EVENT_TEXT_INPUT:
            case SDL_EventType.SDL_EVENT_TEXT_EDITING_CANDIDATES:
            case SDL_EventType.SDL_EVENT_MOUSE_MOTION:
            case SDL_EventType.SDL_EVENT_MOUSE_BUTTON_DOWN:
            case SDL_EventType.SDL_EVENT_MOUSE_BUTTON_UP:
            case SDL_EventType.SDL_EVENT_MOUSE_WHEEL:
                _eventChannel.Writer.TryWrite(new Event(e));
                break;
            case SDL_EventType.SDL_EVENT_QUIT:
                _cts.Cancel();
                break;

            // ignore now, will implement in the future
            case SDL_EventType.SDL_EVENT_DID_ENTER_BACKGROUND:
            case SDL_EventType.SDL_EVENT_WILL_ENTER_FOREGROUND:
            case SDL_EventType.SDL_EVENT_AUDIO_DEVICE_ADDED:
            case SDL_EventType.SDL_EVENT_AUDIO_DEVICE_REMOVED:
            case SDL_EventType.SDL_EVENT_AUDIO_DEVICE_FORMAT_CHANGED:
            case SDL_EventType.SDL_EVENT_CAMERA_DEVICE_ADDED:
            case SDL_EventType.SDL_EVENT_CAMERA_DEVICE_REMOVED:
            case SDL_EventType.SDL_EVENT_CAMERA_DEVICE_APPROVED:
            case SDL_EventType.SDL_EVENT_CAMERA_DEVICE_DENIED:
            case SDL_EventType.SDL_EVENT_RENDER_TARGETS_RESET:
            case SDL_EventType.SDL_EVENT_RENDER_DEVICE_RESET:
            case SDL_EventType.SDL_EVENT_RENDER_DEVICE_LOST:
            case SDL_EventType.SDL_EVENT_DISPLAY_ORIENTATION:
            case SDL_EventType.SDL_EVENT_DISPLAY_ADDED:
            case SDL_EventType.SDL_EVENT_DISPLAY_REMOVED:
            case SDL_EventType.SDL_EVENT_DISPLAY_MOVED:
            case SDL_EventType.SDL_EVENT_DISPLAY_DESKTOP_MODE_CHANGED:
            case SDL_EventType.SDL_EVENT_DISPLAY_CURRENT_MODE_CHANGED:
            case SDL_EventType.SDL_EVENT_DISPLAY_CONTENT_SCALE_CHANGED:
            case SDL_EventType.SDL_EVENT_DISPLAY_USABLE_BOUNDS_CHANGED:
            case SDL_EventType.SDL_EVENT_PRIVATE0:
            case SDL_EventType.SDL_EVENT_PRIVATE1:
            case SDL_EventType.SDL_EVENT_PRIVATE2:
            case SDL_EventType.SDL_EVENT_PRIVATE3:
            case SDL_EventType.SDL_EVENT_POLL_SENTINEL:
            case SDL_EventType.SDL_EVENT_USER:
            case SDL_EventType.SDL_EVENT_TERMINATING:
            case SDL_EventType.SDL_EVENT_LOW_MEMORY:
            case SDL_EventType.SDL_EVENT_WILL_ENTER_BACKGROUND:
            case SDL_EventType.SDL_EVENT_DID_ENTER_FOREGROUND:
            case SDL_EventType.SDL_EVENT_LOCALE_CHANGED:
            case SDL_EventType.SDL_EVENT_SYSTEM_THEME_CHANGED:
            case SDL_EventType.SDL_EVENT_WINDOW_METAL_VIEW_RESIZED:
            case SDL_EventType.SDL_EVENT_WINDOW_EXPOSED:
            case SDL_EventType.SDL_EVENT_WINDOW_MOVED:
            case SDL_EventType.SDL_EVENT_KEYBOARD_ADDED:
            case SDL_EventType.SDL_EVENT_KEYBOARD_REMOVED:
            case SDL_EventType.SDL_EVENT_MOUSE_ADDED:
            case SDL_EventType.SDL_EVENT_MOUSE_REMOVED:
            case SDL_EventType.SDL_EVENT_JOYSTICK_ADDED:
            case SDL_EventType.SDL_EVENT_JOYSTICK_REMOVED:
            case SDL_EventType.SDL_EVENT_GAMEPAD_ADDED:
            case SDL_EventType.SDL_EVENT_GAMEPAD_REMOVED:
            case SDL_EventType.SDL_EVENT_GAMEPAD_REMAPPED:
            case SDL_EventType.SDL_EVENT_JOYSTICK_AXIS_MOTION:
            case SDL_EventType.SDL_EVENT_JOYSTICK_BALL_MOTION:
            case SDL_EventType.SDL_EVENT_JOYSTICK_HAT_MOTION:
            case SDL_EventType.SDL_EVENT_JOYSTICK_BUTTON_DOWN:
            case SDL_EventType.SDL_EVENT_JOYSTICK_BUTTON_UP:
            case SDL_EventType.SDL_EVENT_JOYSTICK_BATTERY_UPDATED:
            case SDL_EventType.SDL_EVENT_JOYSTICK_UPDATE_COMPLETE:
            case SDL_EventType.SDL_EVENT_GAMEPAD_AXIS_MOTION:
            case SDL_EventType.SDL_EVENT_GAMEPAD_BUTTON_DOWN:
            case SDL_EventType.SDL_EVENT_GAMEPAD_BUTTON_UP:
            case SDL_EventType.SDL_EVENT_GAMEPAD_TOUCHPAD_DOWN:
            case SDL_EventType.SDL_EVENT_GAMEPAD_TOUCHPAD_MOTION:
            case SDL_EventType.SDL_EVENT_GAMEPAD_TOUCHPAD_UP:
            case SDL_EventType.SDL_EVENT_GAMEPAD_SENSOR_UPDATE:
            case SDL_EventType.SDL_EVENT_GAMEPAD_UPDATE_COMPLETE:
            case SDL_EventType.SDL_EVENT_GAMEPAD_STEAM_HANDLE_UPDATED:
            case SDL_EventType.SDL_EVENT_FINGER_DOWN:
            case SDL_EventType.SDL_EVENT_FINGER_UP:
            case SDL_EventType.SDL_EVENT_FINGER_MOTION:
            case SDL_EventType.SDL_EVENT_FINGER_CANCELED:
            case SDL_EventType.SDL_EVENT_PINCH_BEGIN:
            case SDL_EventType.SDL_EVENT_PINCH_UPDATE:
            case SDL_EventType.SDL_EVENT_PINCH_END:
            case SDL_EventType.SDL_EVENT_CLIPBOARD_UPDATE:
            case SDL_EventType.SDL_EVENT_DROP_FILE:
            case SDL_EventType.SDL_EVENT_DROP_TEXT:
            case SDL_EventType.SDL_EVENT_DROP_BEGIN:
            case SDL_EventType.SDL_EVENT_DROP_COMPLETE:
            case SDL_EventType.SDL_EVENT_DROP_POSITION:
            case SDL_EventType.SDL_EVENT_SENSOR_UPDATE:
            case SDL_EventType.SDL_EVENT_PEN_PROXIMITY_IN:
            case SDL_EventType.SDL_EVENT_PEN_PROXIMITY_OUT:
            case SDL_EventType.SDL_EVENT_PEN_DOWN:
            case SDL_EventType.SDL_EVENT_PEN_UP:
            case SDL_EventType.SDL_EVENT_PEN_BUTTON_DOWN:
            case SDL_EventType.SDL_EVENT_PEN_BUTTON_UP:
            case SDL_EventType.SDL_EVENT_PEN_MOTION:
            case SDL_EventType.SDL_EVENT_PEN_AXIS:
            case SDL_EventType.SDL_EVENT_WINDOW_CLOSE_REQUESTED:
            case SDL_EventType.SDL_EVENT_WINDOW_HIT_TEST:
            case SDL_EventType.SDL_EVENT_WINDOW_ICCPROF_CHANGED:
            case SDL_EventType.SDL_EVENT_WINDOW_DISPLAY_CHANGED:
            case SDL_EventType.SDL_EVENT_WINDOW_DISPLAY_SCALE_CHANGED:
            case SDL_EventType.SDL_EVENT_WINDOW_SAFE_AREA_CHANGED:
            case SDL_EventType.SDL_EVENT_WINDOW_OCCLUDED:
            case SDL_EventType.SDL_EVENT_WINDOW_DESTROYED:
            case SDL_EventType.SDL_EVENT_WINDOW_HDR_STATE_CHANGED:
            case SDL_EventType.SDL_EVENT_KEYMAP_CHANGED:
            case SDL_EventType.SDL_EVENT_SCREEN_KEYBOARD_SHOWN:
            case SDL_EventType.SDL_EVENT_SCREEN_KEYBOARD_HIDDEN:
            default:
                break;
        }
    }

    private static string[] GetVulkanInstanceExtensions()
    {
        uint count;
        var strings = SDL3.SDL_Vulkan_GetInstanceExtensions(&count);
        var extensions = new string[count];
        for (var i = 0; i < count; i++)
        {
            extensions[i] = Marshal.PtrToStringUTF8((nint)strings[i])!;
        }

        return extensions;
    }

    private static nint CreateVulkanSurface(SDL_Window* window, nint instanceHandle)
    {
        VkSurfaceKHR_T* surface = null;

        if (!SDL3.SDL_Vulkan_CreateSurface(window, (VkInstance_T*)instanceHandle, null, &surface))
            throw new Exception("SDL: failed to create vulkan surface");

        return surface != null ? (nint)surface : 0;
    }

    private static (int, int) GetClientSize(SDL_Window* window)
    {
        var flags = SDL3.SDL_GetWindowFlags(window);
        if ((flags & SDL_WindowFlags.SDL_WINDOW_MINIMIZED) != 0) return (0, 0);

        int w, h;
        SDL3.SDL_GetWindowSize(window, &w, &h);
        return (w, h);
    }

    public static float GetDisplayRefreshRate(SDL_Window* window)
    {
        var displayId = SDL3.SDL_GetDisplayForWindow(window);
        var mode = SDL3.SDL_GetCurrentDisplayMode(displayId);
        return mode->refresh_rate;
    }
}