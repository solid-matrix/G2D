using SDL;

namespace G2D;

public ref struct PenAxisEvent
{
    // TODO

    internal ref SDL_PenAxisEvent _event;

    internal PenAxisEvent(ref SDL_Event e)
    {
        _event = ref e.paxis;
    }

    public EventType EventType => (EventType)_event.type;
}