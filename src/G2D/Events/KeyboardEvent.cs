using SDL;

namespace G2D;

public readonly ref struct KeyboardEvent
{
    internal readonly ref SDL_KeyboardEvent _event;

    public Keys Key => (Keys)_event.scancode;

    public VirtualKeys VirtualKey => (VirtualKeys)_event.key;

    public KeyModifiers Modifiers => (KeyModifiers)_event.mod;

    public bool IsDown => _event.down;

    public bool IsUp => !_event.down;

    public bool IsRepeat => _event.repeat;

    internal KeyboardEvent(ref SDL_Event e)
    {
        _event = ref e.key;
    }

    public EventType EventType => (EventType)_event.type;
}