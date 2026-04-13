using SDL;

namespace G2D;

public readonly struct WindowFocusLostEvent
{
    private readonly SDL_Event _e;

    internal WindowFocusLostEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;
}