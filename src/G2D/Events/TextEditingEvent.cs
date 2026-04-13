using System.Runtime.InteropServices;
using SDL;

namespace G2D;

public readonly unsafe struct TextEditingEvent
{
    private readonly SDL_Event _e;

    internal TextEditingEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;

    public string Text => Marshal.PtrToStringUTF8((nint)_e.edit.text) ?? string.Empty;

    public int Start => _e.edit.start;

    public int Length => _e.edit.length;
}