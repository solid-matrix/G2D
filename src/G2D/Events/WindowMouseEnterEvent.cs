using SDL;

namespace G2D;

public readonly struct WindowMouseEnterEvent
{
    private readonly SDL_Event _e;

    internal WindowMouseEnterEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}