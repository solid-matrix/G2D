using SDL;

namespace G2D;

public ref struct JoyHatEvent
{
    // TODO

    internal ref SDL_JoyHatEvent _event;

    internal JoyHatEvent(ref SDL_Event e)
    {
        _event = ref e.jhat;
    }

    public EventType EventType => (EventType)_event.type;
}