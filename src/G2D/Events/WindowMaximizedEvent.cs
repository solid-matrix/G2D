using SDL;

namespace G2D;

public readonly struct WindowMaximizedEvent
{
    private readonly SDL_Event _e;

    internal WindowMaximizedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}