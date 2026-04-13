using System.Runtime.InteropServices;
using SDL;

namespace G2D;

public unsafe partial class G2D
{
    private SDL_Window* _window;

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
            while (SDL3.SDL_PollEvent(&e)) ProcessEvent(ref e);

            if (ShouldClose) break;

            Thread.Sleep(1);
        }

        if (_window != null)
            SDL3.SDL_DestroyWindow(_window);
    }

    private void ProcessEvent(ref SDL_Event e)
    {
        switch (e.Type)
        {
            case SDL_EventType.SDL_EVENT_QUIT:
                _cts.Cancel();
                break;
            case >= SDL_EventType.SDL_EVENT_WINDOW_FIRST and <= SDL_EventType.SDL_EVENT_WINDOW_LAST:
                (_windowWidth, _windowHeight) = GetClientSize(_window);
                break;
        }
    }

    private static string[] GetVulkanInstanceExtensions()
    {
        uint count;
        var strings = SDL3.SDL_Vulkan_GetInstanceExtensions(&count);
        var extensions = new string[count];
        for (var i = 0; i < count; i++) extensions[i] = Marshal.PtrToStringUTF8((nint)strings[i])!;
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