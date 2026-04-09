using SDL;

namespace G2D;

public ref struct JoyButtonEvent
{
    // TODO

    internal ref SDL_JoyButtonEvent _event;

    internal JoyButtonEvent(ref SDL_Event e)
    {
        _event = ref e.jbutton;
    }

    public EventType EventType => (EventType)_event.type;
}