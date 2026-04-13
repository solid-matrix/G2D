using System.Runtime.InteropServices;
using SDL;

namespace G2D;

public readonly unsafe struct TextInputEvent
{
    private readonly SDL_Event _e;

    internal TextInputEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;

    public string Text => Marshal.PtrToStringUTF8((nint)_e.text.text) ?? string.Empty;
}