using SDL;

namespace G2D;

public readonly struct WindowFocusGainedEvent
{
    private readonly SDL_Event _e;

    internal WindowFocusGainedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}