using SDL;

namespace G2D;

public ref struct DisplayEvent
{
    // TODO

    internal ref SDL_DisplayEvent _event;

    internal DisplayEvent(ref SDL_Event e)
    {
        _event = ref e.display;
    }

    public EventType EventType => (EventType)_event.type;
}