using SDL;

namespace G2D;

public ref struct PenButtonEvent
{
    // TODO

    internal ref SDL_PenButtonEvent _event;

    internal PenButtonEvent(ref SDL_Event e)
    {
        _event = ref e.pbutton;
    }

    public EventType EventType => (EventType)_event.type;
}