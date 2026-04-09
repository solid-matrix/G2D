using SDL;

namespace G2D;

public ref struct QuitEvent
{
    // TODO

    internal ref SDL_QuitEvent _event;

    internal QuitEvent(ref SDL_Event e)
    {
        _event = ref e.quit;
    }

    public EventType EventType => (EventType)_event.type;
}