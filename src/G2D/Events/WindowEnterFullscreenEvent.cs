using SDL;

namespace G2D;

public readonly struct WindowEnterFullscreenEvent
{
    private readonly SDL_Event _e;

    internal WindowEnterFullscreenEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}