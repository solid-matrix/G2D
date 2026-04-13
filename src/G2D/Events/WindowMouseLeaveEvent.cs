using SDL;

namespace G2D;

public readonly struct WindowMouseLeaveEvent
{
    private readonly SDL_Event _e;

    internal WindowMouseLeaveEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}