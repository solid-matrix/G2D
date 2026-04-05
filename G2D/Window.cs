using System.Runtime.InteropServices;
using SDL;

namespace G2D;

public sealed unsafe class Window : IDisposable
{
    internal SDL_Window* _handle;

    internal SDL_WindowID _id;

    static Window()
    {
        if (!SDL3.SDL_Init(SDL_InitFlags.SDL_INIT_VIDEO)) throw new NotSupportedException("SDL not support");
    }

    public Window(string title, int width, int height, WindowFlags flags = WindowFlags.None)
    {
        var sdlFlags = (SDL_WindowFlags)flags
                       | SDL_WindowFlags.SDL_WINDOW_HIGH_PIXEL_DENSITY
                       | SDL_WindowFlags.SDL_WINDOW_VULKAN
                       | SDL_WindowFlags.SDL_WINDOW_HIDDEN;

        _handle = SDL3.SDL_CreateWindow(title, width, height, sdlFlags);

        if (_handle == null)
            throw new Exception("SDL: failed to create window" + SDL3.SDL_GetError());

        _id = SDL3.SDL_GetWindowID(_handle);
    }

    void IDisposable.Dispose()
    {
        if (_handle == null) return;
        SDL3.SDL_DestroyWindow(_handle);
        _handle = null;
    }

    public void Show()
    {
        SDL3.SDL_ShowWindow(_handle);
    }

    public void Hide()
    {
        SDL3.SDL_HideWindow(_handle);
    }

    public void Minimize()
    {
        SDL3.SDL_MinimizeWindow(_handle);
    }

    public void Maximize()
    {
        SDL3.SDL_MaximizeWindow(_handle);
    }

    public void Restore()
    {
        SDL3.SDL_RestoreWindow(_handle);
    }

    public Extent2I GetClientExtent()
    {
        int w, h;

        var flags = SDL3.SDL_GetWindowFlags(_handle);
        if ((flags & SDL_WindowFlags.SDL_WINDOW_MINIMIZED) != 0) return new Extent2I(0, 0);

        SDL3.SDL_GetWindowSize(_handle, &w, &h);
        return new Extent2I(w, h);
    }

    public Vector2I GetClientPosition()
    {
        int top, left, bottom, right;
        SDL3.SDL_GetWindowBordersSize(_handle, &top, &left, &bottom, &right);
        int x, y;
        SDL3.SDL_GetWindowPosition(_handle, &x, &y);

        return new Vector2I(x + left, y + top);
    }

    public RectI GetClientRect()
    {
        int top, left, bottom, right;
        SDL3.SDL_GetWindowBordersSize(_handle, &top, &left, &bottom, &right);
        int x, y;
        SDL3.SDL_GetWindowPosition(_handle, &x, &y);
        int w, h;
        SDL3.SDL_GetWindowSize(_handle, &w, &h);
        return new RectI(x + left, y + top, w, h);
    }

    public Vector2I GetWindowPosition()
    {
        int x, y;
        SDL3.SDL_GetWindowPosition(_handle, &x, &y);
        return new Vector2I(x, y);
    }

    public Extent2I GetWindowExtent()
    {
        int top, left, bottom, right;
        SDL3.SDL_GetWindowBordersSize(_handle, &top, &left, &bottom, &right);
        int w, h;
        SDL3.SDL_GetWindowSize(_handle, &w, &h);

        return new Extent2I(w + left + right, h + top + bottom);
    }

    public RectI GetWindowRect()
    {
        int top, left, bottom, right;
        SDL3.SDL_GetWindowBordersSize(_handle, &top, &left, &bottom, &right);
        int x, y;
        SDL3.SDL_GetWindowPosition(_handle, &x, &y);
        int w, h;
        SDL3.SDL_GetWindowSize(_handle, &w, &h);
        return new RectI(x, y, w + left + right, h + top + bottom);
    }

    public void Move(Vector2I location)
    {
        SDL3.SDL_SetWindowPosition(_handle, location.X, location.Y);
    }

    public void SetIcon(byte[] files)
    {
        fixed (byte* pb = files)
        {
            var stream = SDL3.SDL_IOFromMem((nint)pb, (nuint)files.Length);
            var surface = SDL3_image.IMG_Load_IO(stream, true);
            SDL3.SDL_SetWindowIcon(_handle, surface);
            SDL3.SDL_DestroySurface(surface);
        }
    }

    public void HideCursor()
    {
        SDL3.SDL_HideCursor();
    }

    public void ShowCursor()
    {
        SDL3.SDL_ShowCursor();
    }

    public int GetDisplayRefreshRate()
    {
        var displayId = SDL3.SDL_GetDisplayForWindow(_handle);
        var mode = SDL3.SDL_GetCurrentDisplayMode(displayId);
        return (int)MathF.Round(mode->refresh_rate);
    }

    public nint CreateSurface(nint instance)
    {
        VkSurfaceKHR_T* surface = null;

        if (!SDL3.SDL_Vulkan_CreateSurface(_handle, (VkInstance_T*)instance, null, &surface))
            throw new Exception("SDL: failed to create vulkan surface");

        return surface != null ? (nint)surface : 0;
    }

    public static void DestroySurface(nint instance, ulong surface)
    {
        SDL3.SDL_Vulkan_DestroySurface((VkInstance_T*)instance, (VkSurfaceKHR_T*)surface, null);
    }

    public static string[] GetVulkanInstanceExtensions()
    {
        uint count;
        var strings = SDL3.SDL_Vulkan_GetInstanceExtensions(&count);

        var names = new string[count];

        for (var i = 0; i < count; i++) names[i] = Marshal.PtrToStringUTF8((nint)strings[i])!;

        return names;
    }

    public static implicit operator SDL_Window*(Window w)
    {
        return w._handle;
    }
}