using SDL;

namespace G2D;

public ref struct DropEvent
{
    // TODO

    internal ref SDL_DropEvent _event;

    internal DropEvent(ref SDL_Event e)
    {
        _event = ref e.drop;
    }

    public EventType EventType => (EventType)_event.type;
}