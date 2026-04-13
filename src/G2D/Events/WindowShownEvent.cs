using SDL;

namespace G2D;

public readonly struct WindowShownEvent
{
    private readonly SDL_Event _e;

    internal WindowShownEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}