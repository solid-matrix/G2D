using SDL;

namespace G2D;

public readonly struct WindowLeaveFullscreenEvent
{
    private readonly SDL_Event _e;

    internal WindowLeaveFullscreenEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}