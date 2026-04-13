using SDL;

namespace G2D;

public readonly struct WindowMinimizedEvent
{
    private readonly SDL_Event _e;

    internal WindowMinimizedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}