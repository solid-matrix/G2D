using SDL;

namespace G2D;

public ref struct JoyAxisEvent
{
    // TODO

    internal ref SDL_JoyAxisEvent _event;

    internal JoyAxisEvent(ref SDL_Event e)
    {
        _event = ref e.jaxis;
    }

    public EventType EventType => (EventType)_event.type;
}