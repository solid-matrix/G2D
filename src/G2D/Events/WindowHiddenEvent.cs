using SDL;

namespace G2D;

public readonly struct WindowHiddenEvent
{
    private readonly SDL_Event _e;

    internal WindowHiddenEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}