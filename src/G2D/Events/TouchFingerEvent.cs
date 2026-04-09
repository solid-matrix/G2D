using SDL;

namespace G2D;

public ref struct TouchFingerEvent
{
    // TODO

    internal ref SDL_TouchFingerEvent _event;

    internal TouchFingerEvent(ref SDL_Event e)
    {
        _event = ref e.tfinger;
    }

    public EventType EventType => (EventType)_event.type;
}