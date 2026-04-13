using SDL;

namespace G2D;

public readonly struct KeyDownEvent
{
    private readonly SDL_Event _e;

    internal KeyDownEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;

    public Keys Key => (Keys)_e.key.scancode;

    public LogicalKeys LogicalKey => (LogicalKeys)_e.key.key;

    public KeyModifiers Modifiers => (KeyModifiers)_e.key.mod;

    public bool IsRepeat => _e.key.repeat;
}